using Application.Dto;

namespace Application;

public interface IAuth
{
    Task<(bool, object)> Execute(UserDto userDto);
    Task<object> RefreshTokenUpdate(string RefreshToken);
}