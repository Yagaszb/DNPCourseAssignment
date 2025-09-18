using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository)
{
    private readonly IPostRepository PostRepository = postRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("Create Post");
        Console.Write("Enter Title: ");
        string? title = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(title))
        {
            Console.Write("Title cannot be empty. Enter Title: ");
            title = Console.ReadLine();
        }
        
        Console.Write("Enter Content: ");
        string? body = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(body))
        {
            Console.Write("Content cannot be empty. Enter Content: ");
            body = Console.ReadLine();
        }

        Post newPost = new Post
        {
            Title = title,
            Body = body,
            UserId = 1,
            Id = PostRepository.GetMany().Any() ? PostRepository.GetMany().Max(p => p.Id) + 1 : 1
        };

        await PostRepository.AddAsync(newPost);
        Console.WriteLine($"Post '{newPost.Title}' created successfully with ID {newPost.Id}.");
    }
}