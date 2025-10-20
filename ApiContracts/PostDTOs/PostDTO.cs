namespace ApiContracts.PostDTOs;

public class PostDTO
{
    public required int PostId { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; }
}