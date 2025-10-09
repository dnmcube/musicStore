namespace Settings;

public interface IJwtSettings
{
    public string GetJwtSecretKey { get; }
    public string GetJwtIssuer { get; }
    public string GetJwtAudience{ get; }
    public IJwtSettings JwtSettings { get; }
    public string GetConnectionStringPostgers { get; }
}