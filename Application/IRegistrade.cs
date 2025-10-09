using Application.Dto;
using Infrastructure.Frameworks.Models;

namespace Application;

public interface IRegistrade
{
    Task Execute (UserDto user);
}