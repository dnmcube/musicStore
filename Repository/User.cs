namespace Infrastructure.Frameworks.Models;

public class User:BaseModel
{
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public string Token { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? LastName { get; set; }
}