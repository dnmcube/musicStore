using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Frameworks.Models;

public class Basket:BaseModel
{
    public Guest Guest { get; set; }
    [ForeignKey("GuestId")]
    public Guid GuestId { get; set; }
    
    public Products Products { get; set; }
    [ForeignKey("ProductsId")]
    public Guid ProductsId { get; set; }
}