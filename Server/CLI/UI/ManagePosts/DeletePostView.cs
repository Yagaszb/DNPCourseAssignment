using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class DeletePostView(IPostRepository postRepository)
{
    private readonly IPostRepository PostRepository = postRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Delete Post");
        Console.Write("Enter Post ID to delete: ");
        string? input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Enter Post ID to delete: ");
            input = Console.ReadLine();
        }
        int postId = int.Parse(input);
        var post = await PostRepository.GetSingleAsync(postId); // Check if the post exists, is it needed?
        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        await PostRepository.DeleteAsync(postId);
        Console.WriteLine("Post deleted successfully.");
    }
}