using Application;
using Application.Domine;
using Application.Dto;
using Autofac;
using Infrastructure.Frameworks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MusicStore.Controllers;

[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly ILifetimeScope _lifetimeScope;
    
    public RegistrationController( ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    [AllowAnonymous]
    [HttpPost("Registration")]
    public async Task<IActionResult> Registration([FromBody] UserDto user)
    {
        try
        {
            var _registrade = _lifetimeScope.Resolve<IRegistrade>();
            await _registrade.Execute(user);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
   
    }
    
    [AllowAnonymous]
    [HttpGet("Guest")]
    public async Task<IActionResult> Guest([FromBody] UserDto user)
    {
        try
        {
            var _registrade = _lifetimeScope.Resolve<IRegistrade>();
            var id =await _registrade.Guest();
            return Ok(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
   
    }
    
    [AllowAnonymous]
    [HttpPost("Authorization")]
    public async Task<IActionResult> Authorization([FromBody] UserDto user)
    {
        var _auth = _lifetimeScope.Resolve<IAuth>(); 
        (bool flag, object tokenObject) credentialsBool = await _auth.Execute(user);
        if(!credentialsBool.flag) return Unauthorized("Invalid credentials");

        var res = credentialsBool.tokenObject;
        return Ok(res);
    }
    
    [AllowAnonymous]
    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromForm] string refreshToken)
    {
        var _auth = _lifetimeScope.Resolve<IAuth>(); 
        var res = await _auth.RefreshTokenUpdate(refreshToken);
        return Ok(res);
    }

    
    [AllowAnonymous]
    [HttpPost("Products")]
    public async Task<IActionResult> Get(ProductFilterDto dto)
    {
        var _product = _lifetimeScope.Resolve<IProduct>();
        var res =  await _product.Get(dto);
        return Ok(res);
    }
}