using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class UpdatePostView(IPostRepository postRepository)
{
    private IPostRepository PostRepository = postRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Update Post");
        Console.Write("Enter Post ID to update: ");
        string? input = Console.ReadLine();
        while (input is null || !int.TryParse(input, out _))
        {
            Console.Write("Enter Post ID to update: ");
            input = Console.ReadLine();
        }
        int postId = int.Parse(input);
        var post = await PostRepository.GetSingleAsync(postId);
        if (post == null)
        {
            Console.WriteLine($"Post with ID {postId} not found.");
            return;
        }

        Console.Write($"Enter new title (current: {post.Title}): ");
        string? title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title))
        {
            post.Title = title;
        }

        Console.Write($"Enter new body (current: {post.Body}): ");
        string? body = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(body))
        {
            post.Body = body;
        }

        await PostRepository.UpdateAsync(post);
        Console.WriteLine("Post updated successfully.");
    }
    
    
    
}