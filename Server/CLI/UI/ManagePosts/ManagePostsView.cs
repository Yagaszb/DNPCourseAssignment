using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView(
    IUserRepository userRepository,
    IPostRepository postRepository,
    ICommentRepository commentRepository)
{
    private readonly IPostRepository PostRepository = postRepository;
    private readonly IUserRepository UserRepository = userRepository;
    private readonly ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("Manage Posts");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. View All Posts");
            Console.WriteLine("3. Update Post");
            Console.WriteLine("4. Delete Post");
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
                    CreatePostView createPostView = new CreatePostView(PostRepository);
                    await createPostView.ShowAsync();
                    break;
                }
                case "2":
                {
                    ViewAllPostsView viewAllPostsView = new ViewAllPostsView(UserRepository, PostRepository, CommentRepository);
                    await viewAllPostsView.ShowAsync();
                    break;
                }
                case "3":
                {
                    UpdatePostView updatePostView = new UpdatePostView(PostRepository);
                    await updatePostView.ShowAsync();
                    break;
                }
                case "4":
                {
                    DeletePostView deletePostView = new DeletePostView(PostRepository);
                    await deletePostView.ShowAsync();
                    break;
                }
                case "5":
                {
                    ViewSpecificPostView viewSpecificPostView = new ViewSpecificPostView(UserRepository, PostRepository, CommentRepository);
                    await viewSpecificPostView.ShowAsync();
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