namespace Infrastructure.Frameworks.Models;

public class Guest:BaseModel
{
    public string? TempId { get; set; } = Guid.NewGuid().ToString();
}