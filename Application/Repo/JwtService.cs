

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.UserRegistrate.Domain.Contracts;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace Application.UserRegistrate.Repositories;

public class JwtService: IJwtService
{
    private readonly IJwtSettings _settings;

    public JwtService(IJwtSettings settings)
    {
        _settings = settings;
    }

    public string  GenerateToken (string nameIdentifier, string role, Guid Jti)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtSettings.GetJwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var  claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Jti.ToString()),
            new Claim(ClaimTypes.NameIdentifier, nameIdentifier),
            new Claim(ClaimTypes.Role, role)
        };
       

        var securityToken = new JwtSecurityToken(
            issuer: _settings.JwtSettings.GetJwtIssuer,
            audience: _settings.JwtSettings.GetJwtAudience,
            claims: claims,
            signingCredentials: credentials
        );
        var jwttoken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return jwttoken;
    }

 
    
   
}