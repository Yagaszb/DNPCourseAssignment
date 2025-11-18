using System.Net;
using System.Text.Json;
using ApiContracts.CommentDTOs;

namespace BlazorApp.Service;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient _httpClient;
    
    public HttpCommentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<CommentDTO> AddCommentAsync(CreateCommentDTO request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync($"comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<CommentDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<CommentDTO?> GetCommentByIdAsync(int commentId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"comments/{commentId}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (httpResponse.StatusCode == HttpStatusCode.NotFound) return null;
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<CommentDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

    public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"posts/{postId}/comments");
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<CommentDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<CommentDTO>> GetCommentsByUserIdAsync(int userId)
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync($"users/{userId}/comments");
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<CommentDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<List<CommentDTO>> GetAllCommentsAsync()
    {
        HttpResponseMessage httpResponse = await _httpClient.GetAsync("comments");
        string response =  await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<List<CommentDTO>>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task<CommentDTO> UpdateCommentAsync(int id, CommentDTO request)
    {
        HttpResponseMessage httpResponse = await _httpClient.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        httpResponse.EnsureSuccessStatusCode();
        return JsonSerializer.Deserialize<CommentDTO>(response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    public async Task DeleteCommentAsync(int commentId)
    {
        var resp = await _httpClient.DeleteAsync($"comments/{commentId}");
        if (resp.StatusCode == HttpStatusCode.NotFound)
            throw new Exception($"Comment {commentId} not found.");
        resp.EnsureSuccessStatusCode();
    }
}