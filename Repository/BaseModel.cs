namespace Infrastructure.Frameworks.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
    public bool IsDeleted { get; set; }
}