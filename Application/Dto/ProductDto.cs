namespace Application.Dto;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string Image { get; set; }
}