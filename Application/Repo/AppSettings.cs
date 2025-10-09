namespace Controller.Config;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get;  set; }
    public Jwt Jwt { get; set; }
}


public class ConnectionStrings
{
    public string Postgres { get;  set; }
}
public class Jwt
{
    public string Issuer { get;  set; }
    public string Audience { get;   set;}
    public string SecretKey { get;  set; }
}


