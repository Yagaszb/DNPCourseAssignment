using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class DeleteSpecificCommentOnPostView(IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Delete a Specific Comment on a Post");

        Console.WriteLine("Enter Post ID:");
        string? postIdInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(postIdInput) ||
               !int.TryParse(postIdInput, out _))
        {
            Console.WriteLine("Enter Post ID:");
            postIdInput = Console.ReadLine();
        }
        int postId = int.Parse(postIdInput);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.WriteLine("Enter Comment ID to delete:");
        string? commentIdInput = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(commentIdInput) ||
               !int.TryParse(commentIdInput, out _))
        {
            Console.WriteLine("Enter Comment ID to delete:");
            commentIdInput = Console.ReadLine();
        }
        int commentId = int.Parse(commentIdInput);
        var comment = await CommentRepository.GetSingleAsync(commentId);
        if (comment == null || comment.PostId != postId)
        {
            Console.WriteLine("Comment not found for this post.");
            return;
        }
        await CommentRepository.DeleteAsync(commentId);
        Console.WriteLine("Comment deleted successfully.");
    }
}
