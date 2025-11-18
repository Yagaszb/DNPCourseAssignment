using ApiContracts.PostDTOs;

namespace BlazorApp.Service;

public interface IPostService
{
    public Task<PostDTO> AddPostAsync(CreatePostDTO request);
    public Task<PostDTO?> GetPostByIdAsync(int postId);
    public Task<List<PostDTO>> GetPostsByUserIdAsync(int userId);
    public Task<List<PostDTO>> GetAllPostsAsync();
    public Task<List<PostDTO>> GetPostsByTitleAsync(string title);
    public Task<PostDTO> UpdatePostAsync(int id, PostDTO request);
    public Task DeletePostAsync(int postId);
}