
using Infrastructure.Frameworks.Models;

namespace Application.UserRegistrate.Domain.Contracts;

public interface IUserRegistrateRepo
{
    Task<Guid> AddUsersRolesAsync(Guid loginId, Guid roleId);
    Task<Guid> AddRoleAsync(string role);
    Task<User?> GetUserByLogin(string login, bool isDeleted = false);
    Task<User?> GetUserById(Guid id, bool isDeleted = false);
    Task<Guid> AddUsersAsync(User userModel);
    Task<Guid> AddGuestAsync();
    Task UpdateUserAsync(User userModel);
    Task<List<User>> GetUsers(bool isDeleted);
    Task<Role?> GetRole(bool isDeleted, string roleName);
    Task<List<Role>> GetRoles(bool isDeleted);
    Task SaveChangesAsync();
}