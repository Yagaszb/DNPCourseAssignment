using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    
    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }
    
    public async Task RunAsync()
    {
        Console.WriteLine("CLI app is running...");
        while (true)
        {
            Console.WriteLine("1. Manage Posts");
            Console.WriteLine("2. Manage Users");
            Console.WriteLine("3. Manage Comments");
            Console.WriteLine("0. Exit");
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
                    ManagePostsView managePostsView = new ManagePostsView(userRepository, postRepository, commentRepository);
                    await managePostsView.ShowAsync();
                    break;
                }
                case "2":
                {
                    ManageUsersView manageUsersView = new ManageUsersView(userRepository, postRepository, commentRepository);
                    await manageUsersView.ShowAsync();
                    break;
                }
                case "3":
                {
                    ManageCommentsView manageCommentsView = new ManageCommentsView(userRepository, postRepository, commentRepository);
                    await manageCommentsView.ShowAsync();
                    break;
                }
                case "0":
                {
                    Console.WriteLine("Exiting...");
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