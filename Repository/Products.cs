namespace Infrastructure.Frameworks.Models;

public class Products:BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public bool IsSclad { get; set; }
    public decimal Price { get; set; }
    public string? Image  { get; set; }
}