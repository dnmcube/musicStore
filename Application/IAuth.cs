using Application.Dto;

namespace Application;

public interface IAuth
{
    Task<(bool, string)> Execute(UserDto userDto);
}