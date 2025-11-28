namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }

    // FKs
    public int PostId { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public Post Post { get; set; }
    public User User { get; set; }

    private Comment() { }    // for EFC

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
    }

    public Comment(int id, string body, int postId, int userId)
        : this(body, postId, userId)
    {
        Id = id;
    }
}