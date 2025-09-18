using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ViewSpecificPostView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.Write("Enter Post ID to view: ");
        string? input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out _))
        {
            Console.Write("Invalid input. Please enter a valid Post ID: ");
            input = Console.ReadLine();
        }

        int postId = int.Parse(input);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        var user = await UserRepository.GetSingleAsync(post.UserId);
        var comments = CommentRepository.GetMany().Where(c => c.PostId == post.Id).ToList();

        Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Author: {user.Username}, Comments: {comments.Count}");
        Console.WriteLine($"Content: {post.Body}");
        Console.WriteLine($"Comments: ");
        foreach (var comment in comments)
        {
            var commentUser = await UserRepository.GetSingleAsync(comment.UserId);
            Console.WriteLine($"\n {comment.Body} (by {commentUser.Username})");
        }
        Console.WriteLine(new string('-', 40));
    }
}