using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class DeleteAllCommentsOnPostView(IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Delete All Comments on a Post");

        Console.Write("Enter the ID of the post to delete all comments from (or '0' to cancel): ");
        string? input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Enter the ID of the post to delete all comments from (or '0' to cancel): ");
            input = Console.ReadLine();
        }

        int postId = int.Parse(input);
        if (postId == 0)
        {
            return;
        }

        var selectedPost = await PostRepository.GetSingleAsync(postId);
        if (selectedPost == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        var commentsToDelete = CommentRepository.GetMany().Where(c => c.PostId == postId).ToList();
        if (!commentsToDelete.Any())
        {
            Console.WriteLine("No comments to delete for this post.");
            return;
        }

        foreach (var comment in commentsToDelete)
        {
            await CommentRepository.DeleteAsync(comment.Id);
        }

        Console.WriteLine($"Deleted {commentsToDelete.Count} comments from post ID {postId}.");
    }
}