using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Frameworks.Models;

public class BoughtUserProduct:BaseModel
{

    public User User { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }

    public int ProductCount { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductPrice { get; set; }
}