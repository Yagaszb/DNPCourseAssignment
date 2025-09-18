using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("Manage Users");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. View All Users");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Delete User");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Select an option: ");
            string? input = Console.ReadLine();
            while (input is null)
            {
                Console.Write("Select an option: ");
                input = Console.ReadLine();
            }
            switch (input)
            {
                case "1":
                {
                    CreateUserView createUserView = new CreateUserView(UserRepository);
                    await createUserView.ShowAsync();
                    break;
                }
                case "2":
                {
                    ViewAllUsersView viewAllUsersView = new ViewAllUsersView(UserRepository, PostRepository, CommentRepository);
                    await viewAllUsersView.ShowAsync();
                    break;
                }
                case "3":
                {
                    UpdateUserView updateUserView = new UpdateUserView(UserRepository);
                    await updateUserView.ShowAsync();
                    break;
                }
                case "4":
                {
                    DeleteUserView deleteUserView = new DeleteUserView(UserRepository);
                    await deleteUserView.ShowAsync();
                    break;
                }
                case "0":
                {
                    return;
                }
                default:
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
                }
            }
        }
    }
}