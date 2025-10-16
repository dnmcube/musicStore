using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Application.Dto;
using Application.UserRegistrate.Domain.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class Auth:IAuth 
{
    private readonly IUserRegistrateRepo _userRegistrateRepo;
    private readonly IJwtService _jwtService;
    public Auth(IUserRegistrateRepo userRegistrateRepo, IJwtService jwtService)
    {
        _userRegistrateRepo = userRegistrateRepo;
        _jwtService = jwtService;
    }

    public async Task<(bool, object)> Execute(UserDto userDto)
    {
        var model = await _userRegistrateRepo.GetUserByLogin(userDto.Login);
        if (model == null)
            return (false, null);
        
        var validPassword = BCrypt.Net.BCrypt.Verify(userDto.Password, model.PasswordHash);
        if (!validPassword)
            return (false, null);

        var res =  await RefreshTokenUpdate(model.RefreshToken);
        return (true, res);
    }


    public async Task<object> RefreshTokenUpdate(string RefreshToken)
    {
       (string Token, string RefreshToken, string error) res =  await _jwtService.RefreshTokenGet(RefreshToken);
       if (string.IsNullOrWhiteSpace(res.error))
       {
           return new
           {
               token = res.Token,
               refreshToken = res.RefreshToken
           };
       }

       return res.error;
    }
}