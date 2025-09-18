using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ManageCommentsView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("Manage Comments");
            Console.WriteLine("1. Add Comment To A Post");
            Console.WriteLine("2. View All Comments On A Post");
            Console.WriteLine("3. View A Specific Comment On A Post");
            Console.WriteLine("4. Delete All Comments On A Post");
            Console.WriteLine("5. Delete A Specific Comment On A Post");
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
                    AddCommentToPostView addCommentToPostView = new AddCommentToPostView(UserRepository, PostRepository, CommentRepository);
                    await addCommentToPostView.ShowAsync();
                    break;
                }
                case "2":
                {
                    ViewAllCommentsOnPostView viewAllCommentsOnPostView = new ViewAllCommentsOnPostView(UserRepository, PostRepository, CommentRepository);
                    await viewAllCommentsOnPostView.ShowAsync();
                    break;
                }
                case "3":
                {
                    ViewSpecificCommentOnPostView viewSpecificCommentOnPostView = new ViewSpecificCommentOnPostView(UserRepository, PostRepository, CommentRepository);
                    await viewSpecificCommentOnPostView.ShowAsync();
                    break;
                }
                case "4":
                {
                    DeleteAllCommentsOnPostView deleteAllCommentsOnPostView = new DeleteAllCommentsOnPostView(PostRepository, CommentRepository);
                    await deleteAllCommentsOnPostView.ShowAsync();
                    break;
                }
                case "5":
                {
                    DeleteSpecificCommentOnPostView
                        deleteSpecificCommentOnPostView =
                            new DeleteSpecificCommentOnPostView(PostRepository, CommentRepository);
                    await deleteSpecificCommentOnPostView.ShowAsync();
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