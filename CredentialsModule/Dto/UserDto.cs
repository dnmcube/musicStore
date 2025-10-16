namespace Application.Dto;

public class UserDto
{
    public Guid GuestId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }

}