using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Frameworks.Models;

public class UserRole :BaseModel
{
    [ForeignKey(nameof(UserId))]
    public User? User { get; set;}
    public Guid? UserId { get; set;}


    
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set;}
    public Guid RoleId { get; set;}

}