using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class AddCommentToPostView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;

    public async Task ShowAsync()
    {
        Console.WriteLine("Add Comment to Post");
        Console.Write("Enter Post ID: ");
        string? postIdInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(postIdInput) || !int.TryParse(postIdInput, out _))
        {
            Console.Write("Invalid input. Enter a valid Post ID: ");
            postIdInput = Console.ReadLine();
        }
        int postId = int.Parse(postIdInput);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        Console.Write("Enter User ID (author of the comment): ");
        string? userIdInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(userIdInput) || !int.TryParse(userIdInput, out _))
        {
            Console.Write("Invalid input. Enter a valid User ID: ");
            userIdInput = Console.ReadLine();
        }
        int userId = int.Parse(userIdInput);
        var user = await UserRepository.GetSingleAsync(userId);
        if (user == null)
        {
            Console.WriteLine($"User with ID {userId} not found.");
            return;
        }

        Console.Write("Enter Comment Body: ");
        string? body = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(body))
        {
            Console.Write("Comment body cannot be empty. Enter Comment Body: ");
            body = Console.ReadLine();
        }

        var newComment = new Comment
        {
            PostId = postId,
            UserId = userId,
            Body = body,
            Id = CommentRepository.GetMany().Any() ? CommentRepository.GetMany().Max(p => p.Id) + 1 : 1
        };

        await CommentRepository.AddAsync(newComment);
        Console.WriteLine("Comment added successfully.");
    }
}