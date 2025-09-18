using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ViewAllCommentsOnPostView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
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
        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        var comments = CommentRepository.GetMany().Where(c => c.PostId == postId).ToList();
        if (!comments.Any())
        {
            Console.WriteLine("No comments available for this post.");
            return;
        }

        Console.WriteLine($"Comments for Post ID {postId} - {post.Title}:");
        foreach (var comment in comments)
        {
            var user = await UserRepository.GetSingleAsync(comment.UserId);
            Console.WriteLine($"ID: {comment.Id}, Author: {user.Username}");
            Console.WriteLine($"Content: {comment.Body}");
            Console.WriteLine(new string('-', 40));
        }
    }
}