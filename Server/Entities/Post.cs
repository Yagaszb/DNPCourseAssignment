namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }

    // FK + navigation to User (author)
    public int UserId { get; set; }
    public User User { get; set; }

    // Navigation to comments
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    private Post() { }   // for EFC

    public Post(string title, string body, int userId)
    {
        Title = title;
        Body = body;
        UserId = userId;
    } 
    
}