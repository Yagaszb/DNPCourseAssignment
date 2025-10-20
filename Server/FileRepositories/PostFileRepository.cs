using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        int maxId = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        post.Id = maxId;
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.Id}' not found");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);
        postAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postAsJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return post;
    }

    public async Task<Post> GetSingleAsync(string title)
    {
        string postAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        Post? post = posts.SingleOrDefault(p => p.Title == title);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with title '{title}' not found");
        }

        return post;
    }

    public IQueryable<Post> GetMany()
    {
        string postAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson) !;
        return posts.AsQueryable();
    }
}