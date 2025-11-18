using System.Net;
using System.Text;
using System.Text.Json;
using ApiContracts.UserDTOs;

namespace BlazorApp.Service;

public class HttpUserService : IUserService
{
    private readonly HttpClient _httpClient;
    
    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserDTO> AddUserAsync(CreateUserDTO request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<UserDTO?> GetUserByIdAsync(int userId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"users/{userId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            if (string.IsNullOrEmpty(response) || httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        HttpResponseMessage httpResponse =  await _httpClient.GetAsync("users");
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<UserDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<List<UserDTO>> GetUsersByUserNameAsync(string userName)
    {
        var encoded = Uri.EscapeDataString(userName);
        HttpResponseMessage httpResponse =  await _httpClient.GetAsync($"users?userName={encoded}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<UserDTO>();
            }
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<List<UserDTO>>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO request)
    {
        HttpResponseMessage httpResponse =  await _httpClient.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task DeleteUserAsync(int userId)
    {
        var resp = await _httpClient.DeleteAsync($"users/{userId}");
        if (resp.StatusCode == HttpStatusCode.NotFound)
            throw new Exception($"User {userId} not found.");
        resp.EnsureSuccessStatusCode();
    }
}