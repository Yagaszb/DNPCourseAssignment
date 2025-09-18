using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class DeleteUserView(IUserRepository userRepository)
{
    private IUserRepository UserRepository = userRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Delete User");
        Console.Write("Enter user ID to delete: ");
        string? inputId = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(inputId) || !int.TryParse(inputId, out _))
        {
            Console.Write("Enter user ID to delete: ");
            inputId = Console.ReadLine();
        }
        int userId = int.Parse(inputId);
        var user = await UserRepository.GetSingleAsync(userId); // Check if the user exists, is it needed?
        if (user == null)
        {
            Console.WriteLine($"User with ID {userId} not found.");
            return;
        }
        
        await UserRepository.DeleteAsync(userId);
        Console.WriteLine($"User ID {userId} deleted successfully.");
    }
}