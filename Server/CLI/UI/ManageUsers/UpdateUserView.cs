using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class UpdateUserView(IUserRepository userRepository)
{
    private IUserRepository UserRepository = userRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Update User");
        Console.Write("Enter user ID to update: ");
        string? inputId = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(inputId) || !int.TryParse(inputId, out _))
        {
            Console.Write("Enter user ID to update: ");
            inputId = Console.ReadLine();
        }
        int userId = int.Parse(inputId);
        var user = await UserRepository.GetSingleAsync(userId);
        if (user == null)
        {
            Console.WriteLine($"User with ID {userId} not found.");
            return;
        }
        
        Console.Write($"Enter new username (current: {user.Username}): ");
        string? newUsername = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(newUsername))
        {
            Console.Write($"Enter new username (current: {user.Username}): ");
            newUsername = Console.ReadLine();
        }
        
        Console.Write("Enter new password: ");
        string? newPassword = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(newPassword))
        {
            Console.Write("Enter new password: ");
            newPassword = Console.ReadLine();
        }
        
        user.Username = newUsername;
        user.Password = newPassword;
        
        await UserRepository.UpdateAsync(user);
        Console.WriteLine($"User ID {userId} updated successfully.");
    }
}