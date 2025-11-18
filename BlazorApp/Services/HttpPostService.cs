using System.Net;
using System.Text.Json;
using ApiContracts.PostDTOs;

namespace BlazorApp.Service;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;
    
    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PostDTO> AddPostAsync(CreatePostDTO request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("posts", request);   
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Error from API ({httpResponse.StatusCode}): {response}");
        }        
        return JsonSerializer.Deserialize<PostDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<PostDTO?> GetPostByIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/{postId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (httpResponse.StatusCode == HttpStatusCode.NotFound) return null;
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PostDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<List<PostDTO>> GetPostsByUserIdAsync(int userId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/users/{userId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<PostDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<PostDTO>> GetAllPostsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("posts");
        string response =  await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<PostDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<PostDTO>> GetPostsByTitleAsync(string title)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/title?title={Uri.EscapeDataString(title)}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<List<PostDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<PostDTO> UpdatePostAsync(int id, PostDTO request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<PostDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task DeletePostAsync(int postId)
    {
        var resp = await _httpClient.DeleteAsync($"posts/{postId}");
        resp.EnsureSuccessStatusCode();
    }
}