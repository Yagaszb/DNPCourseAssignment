using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ViewSpecificCommentOnPostView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.Write("Enter Post ID to view comments: ");
        string? input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Invalid input. Enter Post ID to view comments: ");
            input = Console.ReadLine();
        }
        int postId = int.Parse(input);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post is null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }
        Console.Write("Enter Comment ID to view comment: ");
        input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Invalid input. Enter Comment ID to view comment: ");
            input = Console.ReadLine();
        }
        int commentId = int.Parse(input);
        var comment = await CommentRepository.GetSingleAsync(commentId);
        if (comment is null)
        {
            Console.WriteLine("No comment available for this post.");
            return;
        }
        
        Console.WriteLine($"Comments for Post ID {postId}:");
        var user = await UserRepository.GetSingleAsync(comment.UserId);
        Console.WriteLine($"ID: {comment.Id}, Author: {user.Username}");
        Console.WriteLine($"Content: {comment.Body}");
        Console.WriteLine(new string('-', 40));
    }
}