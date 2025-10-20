namespace ApiContracts.CommentDTOs;

public class CommentDTO
{
    public required int CommentId { get; set; }
    public required string Body { get; set; }
    public required int PostId { get; set; }
    public required int UserId { get; set; }
}