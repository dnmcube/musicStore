using Application.Dto;
using Application.UserRegistrate.Domain.Contracts;

namespace Application;

public class Auth:IAuth 
{
    private readonly IUserRegistrateRepo _userRegistrateRepo;
    public Auth(IUserRegistrateRepo userRegistrateRepo)
    {
        _userRegistrateRepo = userRegistrateRepo;
    }

    public async Task<(bool, string)> Execute(UserDto userDto)
    {
        var model = await _userRegistrateRepo.GetUserByLogin(userDto.Login);
        if (model == null)
            return (false, "");
        
        var validPassword = BCrypt.Net.BCrypt.Verify(userDto.Password, model.PasswordHash);
        if (!validPassword)
            return (false, "");

        return (true, model.Token);
    }
}