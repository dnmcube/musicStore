using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Frameworks.Models;

public class UserRole :BaseModel
{
    [ForeignKey(nameof(LoginId))]
    public User? User { get; set;}
    public Guid? LoginId { get; set;}


    
    [ForeignKey(nameof(RoleId))]
    public Role Role { get; set;}
    public Guid RoleId { get; set;}

}