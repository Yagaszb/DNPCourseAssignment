using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class UpdateCommentView(IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.Write("Enter Post ID to update comment: ");
        string? input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Invalid input. Enter Post ID to update comment: ");
            input = Console.ReadLine();
        }
        int postId = int.Parse(input);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post is null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }
        Console.Write("Enter Comment ID to update comment: ");
        input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Invalid input. Enter Comment ID to update comment: ");
            input = Console.ReadLine();
        }
        int commentId = int.Parse(input);
        var comment = await CommentRepository.GetSingleAsync(commentId);
        if (comment is null)
        {
            Console.WriteLine("No comment available for this post.");
            return;
        }
        
        Console.Write("Enter new content for the comment: ");
        string? newContent = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(newContent))
        {
            Console.Write("Content cannot be empty. Enter new content for the comment: ");
            newContent = Console.ReadLine();
        }
        
        comment.Body = newContent;
        await CommentRepository.UpdateAsync(comment);
        Console.WriteLine("Comment updated successfully.");
    }
}