using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ViewAllUsersView(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
{
    private IUserRepository UserRepository = userRepository;
    private IPostRepository PostRepository = postRepository;
    private ICommentRepository CommentRepository = commentRepository;
    
    public async Task ShowAsync()
    {
        Console.WriteLine("All Users:");
        var users = UserRepository.GetMany().ToList();
        if (!users.Any())
        {
            Console.WriteLine("No users available.");
            return;
        }

        foreach (var user in users)
        {
            var posts = PostRepository.GetMany().Where(p => p.UserId == user.Id).ToList();
            var comments = CommentRepository.GetMany().Where(c => c.UserId == user.Id).ToList();            
            Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Posts: {posts.Count}, Comments: {comments.Count}");
            Console.WriteLine(new string('-', 40));
        }
    }
}