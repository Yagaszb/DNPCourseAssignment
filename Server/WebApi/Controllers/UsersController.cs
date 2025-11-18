using ApiContracts.UserDTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<UserDTO>> GetUserById
    (
        [FromRoute] int userId
    )
    {
        try
        { 
            User user = await userRepository.GetSingleAsync(userId);
            
            UserDTO response = new()
            {
                Id = user.Id,
                UserName = user.Username
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
    
    [HttpGet]
    public ActionResult<List<UserDTO>> GetUsers([FromQuery] string? userName)
    {
        var query = userRepository.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
            query = query.Where(u => 
                u.Username.Equals(userName.Trim(), StringComparison.OrdinalIgnoreCase));

        var users = query.Select(u => new UserDTO { Id = u.Id, UserName = u.Username }).ToList();

        /*if (users.Count == 0 && !string.IsNullOrWhiteSpace(userName))
            return NotFound($"User '{userName}' not found.");*/ //could be empty 

        return Ok(users);
    }

    
    /*[HttpGet("all")]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers()
    {
        List<UserDTO> response = userRepository.GetMany()
            .Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.Username
            })
            .ToList();
        return Ok(response);
    }*/
    
    /*[HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetUsersByUserName
    (
        [FromQuery] string? userName
    )
    {
        userName = userName?.Trim();
        if (string.IsNullOrEmpty(userName))
            return BadRequest("Query 'userName' is required.");
        List<UserDTO> response = userRepository.GetMany()
            .Where(user => user.Username.Equals(userName, StringComparison.OrdinalIgnoreCase))
            .Select(user => new UserDTO
            {
                Id = user.Id,
                UserName = user.Username
            })
            .ToList();
        if(response.Count == 0)
            return NotFound($"User with name {userName} not found.");
        return Ok(response);
    }
    */
    
    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser
    (
        [FromBody] CreateUserDTO request
    )
    {
        try
        {
            await VerifyUserNameAvailableAsync(request.UserName);
            User user = new User(request.UserName.Trim(), request.Password);
            User created = await userRepository.AddAsync(user);
            UserDTO response = new()
            {
                Id = created.Id,
                UserName = created.Username
            };
            return Created($"/users/{response.Id}", response);
        }
        catch (ArgumentException ex) // username taken
        {
            return Conflict(ex.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("{userId:int}")]
    public async Task<ActionResult<UserDTO>> UpdateUser
    (
        [FromRoute] int userId,
        [FromBody] UpdateUserDTO request
    )
    {
        if (userId != request.Id)
            return BadRequest("User ID in route and body do not match.");
        
        try
        {
            User existing = await userRepository.GetSingleAsync(userId);
            if (existing.Username != request.UserName)
            {
                await VerifyUserNameAvailableAsync(request.UserName);
            }
            existing.Username = request.UserName;
            existing.Password = request.Password;
            User updated = await userRepository.UpdateAsync(existing);

            var response = new UserDTO { Id = updated.Id, UserName = updated.Username };
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult> DeleteUser
    (
        [FromRoute] int userId
    )
    {
        try
        {
            
            await userRepository.DeleteAsync(userId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    private async Task VerifyUserNameAvailableAsync(string userName)
    {
        User? existing = await userRepository.GetSingleAsync(userName);
        if (existing is not null)
        {
            throw new ArgumentException($"Username '{userName}' is already taken.");
        }
    }
}