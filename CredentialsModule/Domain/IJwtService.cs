using Infrastructure.Frameworks.Models;

namespace  Application.UserRegistrate.Domain.Contracts;

public interface IJwtService
{
    // AuthorizationTokenDto GenerateToken(string PhoneNumber, string role,
    //     Guid userid, AuthorizationApiKeyDto? apiKeyDto = null);

   public string GenerateToken(string nameIdentifier, string role, Guid Jti);
   string GenerateRefreshToken(string nameIdentifier, string role, Guid Jti);
   Task<(string?, string?, string?)> RefreshTokenGet(string refreshToken);
}