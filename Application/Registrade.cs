using System.Text;
using Application.Dto;
using Application.UserRegistrate.Domain.Contracts;
using Domain.Enums;
using Infrastructure.Frameworks.Models;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace Application;

public class Registrade : IRegistrade
{
    private readonly IJwtSettings _setting;
    private readonly IJwtService _jwtService;
    private readonly IUserRegistrateRepo _userRegistrateRepo;

    public Registrade(IUserRegistrateRepo userRegistrateRepo, IJwtSettings setting,  IJwtService jwtService)
    {
        _userRegistrateRepo = userRegistrateRepo;
        _setting = setting;
        _jwtService = jwtService;
    }
    
    public async Task Execute (UserDto userdto)
    {
        var user = new User()
        {
            Login = userdto.Login,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userdto.Password)
        };
        var model = await _userRegistrateRepo.GetUserByLogin(userdto.Login);
        if (model != null)
        {
            throw new Exception($"Такой логин уже существует в системе ({userdto.Login})");
        }
        
        var Id = await _userRegistrateRepo.AddUsersAsync(user);
        await _userRegistrateRepo.SaveChangesAsync();
        
        var role = await _userRegistrateRepo.GetRole(false, RolesEnum.Admin.ToString());
        
        var token = _jwtService.GenerateToken(user.Login, RolesEnum.Client.ToString(), Id);
        user.Token = token;
        user.Id = Id;


        await _userRegistrateRepo.UpdateUserAsync(user);

        await _userRegistrateRepo.AddUsersRolesAsync(Id, role.Id);

        await _userRegistrateRepo.SaveChangesAsync();
    }
}