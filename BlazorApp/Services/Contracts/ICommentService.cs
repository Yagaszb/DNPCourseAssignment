using ApiContracts.CommentDTOs;

namespace BlazorApp.Service;

public interface ICommentService
{
    public Task<CommentDTO> AddCommentAsync(CreateCommentDTO request);
    public Task<CommentDTO?> GetCommentByIdAsync(int commentId);
    public Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId);
    public Task<List<CommentDTO>> GetCommentsByUserIdAsync(int userId);
    public Task<List<CommentDTO>> GetAllCommentsAsync();
    public Task<CommentDTO> UpdateCommentAsync(int id, CommentDTO request);
    public Task DeleteCommentAsync(int commentId);
}