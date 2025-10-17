

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.UserRegistrate.Domain.Contracts;
using Domain.Enums;
using Infrastructure.Frameworks.Models;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace Application.UserRegistrate.Repositories;

public class JwtService: IJwtService
{
    private readonly IJwtSettings _settings;
    private readonly IUserRegistrateRepo _userRegistrateRepo;
    public JwtService(IJwtSettings settings, IUserRegistrateRepo userRegistrateRepo)
    {
        _settings = settings;
        _userRegistrateRepo = userRegistrateRepo;
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
            expires: DateTime.UtcNow.AddMinutes(1), 
            signingCredentials: credentials
        );
        var jwttoken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return jwttoken;
    }
    public string  GenerateRefreshToken (string nameIdentifier, string role, Guid Jti)
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
            notBefore: DateTime.UtcNow,                 // Токен активен сразу
            expires: DateTime.UtcNow.AddDays(7),    // Время жизни токена
            signingCredentials: credentials
        );
        var jwttoken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return jwttoken;
    }

    public async Task<(string?, string?, Guid?, string?)> RefreshTokenGet(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return ("","",null, "Refresh token missing");

        var tokenHandler = new JwtSecurityTokenHandler();

        // 1) Валидируем подпись refresh-token (и проверяем срок)
        TokenValidationParameters validationParams = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true, // Обычно проверяем expiry для refresh
            ValidIssuer = _settings.GetJwtIssuer,
            ValidAudience = _settings.GetJwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.GetJwtSecretKey))
        };
        
        ClaimsPrincipal principal;
        SecurityToken validatedToken;
        try
        {
            principal = tokenHandler.ValidateToken(refreshToken, validationParams, out validatedToken);
        }
        catch (SecurityTokenException)
        {
            return ("","",null,"Invalid refresh token (signature/expired)");
        }
        
        // 2) Получаем JTI (у тебя — userId)
        var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrEmpty(jti))
            return ("","",null,"Refresh token does not contain jti");

        // если jti хранит GUID
        if (!Guid.TryParse(jti, out var Id))
            return ("","",null,"Invalid jti format");

        var user = await _userRegistrateRepo.GetUserById(Id);
        // 3) Берём пользователя/запись refresh token в БД и сверяем
        var _user = user.Id == Id;
        if (_user == false) return ("","",null,"User not found");
        
        // Предполагаем, что в таблице Users есть поля RefreshToken и RefreshTokenExpiry
        if (user.RefreshToken != refreshToken || user.ExpiresAt <= DateTime.UtcNow)
            return ("","",null,"Refresh token mismatch or expired");

        // 4) Всё ок — создаём новый access (и rotate refresh)
        var newAccess = GenerateToken(user.Login, RolesEnum.Client.ToString(), user.Id);
        var newRefresh = GenerateRefreshToken(user.Login, RolesEnum.Client.ToString(), user.Id);

        user.Token = newAccess;
        user.RefreshToken = newRefresh;
        
        await _userRegistrateRepo.UpdateUserAsync(user);
        await _userRegistrateRepo.SaveChangesAsync();
        
        
        return (newAccess, newRefresh, user.Id , "");
    }
 
    
   
}