namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }

    public Comment()
    {
        
    }
    
    public Comment(string commentBody, int commentPostId, int commentUserId)
    {
        Body = commentBody;
        PostId = commentPostId;
        UserId = commentUserId;
    }

    public Comment(int postId, int userId, string body, int id)
    {
        PostId = postId;
        UserId = userId;
        Body = body;
        Id = id;
    }
}