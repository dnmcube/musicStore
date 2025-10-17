using Application.UserRegistrate.Domain.Contracts;
using Infrastructure.Frameworks.DataBase;
using Infrastructure.Frameworks.DataBase.BaseRepositories;
using Infrastructure.Frameworks.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.UserRegistrate.Repositories;

public class UserRegistrateRepo:BaseRepo,  IUserRegistrateRepo
{
    public UserRegistrateRepo(Context context):base(context)
    {
    }
    public async Task<Guid> AddUsersRolesAsync(Guid loginId, Guid roleId)
    {
        var res = await _context.Set<UserRole>().Where(x => x.IsDeleted == false && x.UserId == loginId && x.RoleId == roleId).FirstOrDefaultAsync();
        if (res != null) return res.Id;
       return await AddAsync(new UserRole
       {
           UserId = loginId,
           RoleId = roleId,
       });
    }
    public async Task<Guid> AddRoleAsync(string role)
    {
        return await AddAsync(new Role
        {
            Name = role
        });
    }
    public async Task<Guid> AddUsersAsync(User userModel)
    {
        return  await AddAsync(new User
        {
            Login = userModel.Login,
            PasswordHash = userModel.PasswordHash,
            Email = userModel.Email,
            Token = userModel.Token ?? "",
            FirstName = userModel.FirstName,
            SecondName = userModel.SecondName,
            LastName = userModel.LastName
        });
    }

    public async Task<Guid> AddGuestAsync()
    {
        var id =  await AddAsync(new Guest());
        await SaveChangesAsync();
        return id;
    }
    public async Task UpdateUserAsync(User userModel)
    {
         var res = await _context.Set<User>().Where(x => x.IsDeleted == false && x.Id == userModel.Id).FirstOrDefaultAsync();
         res.Login = userModel.Login;
         res.Token = userModel.Token;
         res.RefreshToken = userModel.RefreshToken;
         res.ExpiresAt = userModel.ExpiresAt;
         res.Email = userModel.Email;
         res.FirstName = userModel.FirstName;
         res.SecondName = userModel.SecondName;
         res.LastName = userModel.LastName; 
       await  Update(res);
    }
    public async Task<User?> GetUserByLogin(string login,  bool isDeleted = false)
    {
        return await _context.Set<User>().Where(x => x.IsDeleted == isDeleted && x.Login == login)
        .FirstOrDefaultAsync();
    }
    public async Task<User?> GetUserById(Guid id,  bool isDeleted = false)
    {
        return await _context.Set<User>().Where(x => x.IsDeleted == isDeleted && x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<List<User>> GetUsers(bool isDeleted)
    {
        return await _context.Set<User>().Where(x => x.IsDeleted == isDeleted).ToListAsync();
    }
    public async Task<Role?> GetRole(bool isDeleted, string roleName)
    {
        return await _context.Set<Role>().Where(x => x.Name == roleName && x.IsDeleted == isDeleted).FirstOrDefaultAsync();
    }
    public async Task<List<Role>> GetRoles(bool isDeleted)
    {
        return await _context.Set<Role>().Where(x => x.IsDeleted == isDeleted).ToListAsync();
    }
    
    
    
}

