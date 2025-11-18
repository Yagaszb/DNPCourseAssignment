using ApiContracts.UserDTOs;

namespace BlazorApp.Service;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO request);
    public Task<UserDTO?> GetUserByIdAsync(int userId);
    public Task<List<UserDTO>> GetAllUsersAsync();
    public Task<List<UserDTO>> GetUsersByUserNameAsync(string userName);
    public Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO request);
    public Task DeleteUserAsync(int userId);
}