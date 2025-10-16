namespace Infrastructure.Frameworks.Models;

public abstract class BaseModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}