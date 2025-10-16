namespace Application.Dto;

public class ProductFilterDto
{
    public int Page { get; set; }
    public string Type { get; set; }
    public bool PriceUpDown { get; set; }
}