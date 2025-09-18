using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ViewAllPostsView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private readonly IPostRepository PostRepository = postRepository;
    private readonly IUserRepository UserRepository = userRepository;
    private readonly ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("All Posts:");
        var posts = PostRepository.GetMany().ToList();
        if (!posts.Any())
        {
            Console.WriteLine("No posts available.");
            return;
        }

        foreach (var post in posts)
        {
            var user = await UserRepository.GetSingleAsync(post.UserId);
            var comments = CommentRepository.GetMany().Where(c => c.PostId == post.Id).ToList();            
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, Author: {user.Username}, Comments: {comments.Count}");
            Console.WriteLine($"Content: {post.Body}");
            Console.WriteLine($"Comments: ");
            foreach (var comment in comments)
            {
                Console.WriteLine($"\n {comment.Body} (by User {comment.UserId})");
            }
            Console.WriteLine(new string('-', 40));
        }
    }
}