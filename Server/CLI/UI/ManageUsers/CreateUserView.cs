using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    private IUserRepository UserRepository = userRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Create User");
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(username))
        {
            Console.Write("Enter username: ");
            username = Console.ReadLine();
        }
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(password))
        {
            Console.Write("Enter password: ");
            password = Console.ReadLine();
        }
        
        User newUser = new User
        {
            Id = UserRepository.GetMany().Any() ? UserRepository.GetMany().Max(p => p.Id) + 1 : 1,
            Username = username,
            Password = password
        };
        
        await UserRepository.AddAsync(newUser);
        Console.WriteLine($"User '{username}' created successfully.");
    }
}