namespace Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    // Navigation properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    private User() { }   // for EFC

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public User(int id, string username, string password) : this(username, password)
    {
        Id = id;
    }
}