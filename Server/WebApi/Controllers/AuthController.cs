using ApiContracts.Requests;
using ApiContracts.UserDTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserRepository userRepository) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        User? user = await userRepository.GetSingleAsync(loginRequest.UserName);
        if (user == null)
        {
            return Unauthorized("No user found with such username");
        }

        if (user.Password != loginRequest.Password)
        {
            return Unauthorized("Invalid password");
        }
        
        UserDTO response = new()
        {
            Id = user.Id,
            UserName = user.Username
        };
        
        return Ok(response);
    }
}