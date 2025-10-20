namespace ApiContracts.UserDTOs;

public class UpdateUserDTO
{
    public required int Id { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}