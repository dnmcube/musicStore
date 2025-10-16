using System.Runtime.CompilerServices;
using Settings;
using Microsoft.Extensions.Options;

namespace Controller.Config;

public class Settings: IJwtSettings
{
    private readonly IOptions<AppSettings> _options;
    public Settings(IOptions<AppSettings> options)
    {
        _options = options;
    }
    public IJwtSettings JwtSettings => this;
    

    public string GetJwtSecretKey => _options.Value.Jwt.SecretKey;
    public string GetJwtAudience => _options.Value.Jwt.Audience;
    public string GetJwtIssuer => _options.Value.Jwt.Issuer;
    
    public string GetConnectionStringPostgers => _options.Value.ConnectionStrings.Postgres;

}