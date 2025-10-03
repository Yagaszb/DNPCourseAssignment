using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<User> AddAsync(User user)
    {
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson) !;
        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson) !;
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{user.Id}' not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string userAsJson = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson) !;
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }
        users.Remove(userToRemove);
        userAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, userAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string userAsJson = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson) !;
        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException(
                $"User with ID '{id}' not found");
        }

        return user;
    }

    public IQueryable<User> GetMany()
    {
        string userAsJson = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(userAsJson) !;
        return users.AsQueryable();
    }
}