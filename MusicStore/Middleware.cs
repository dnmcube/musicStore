using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace Controller.Config;
public class Middleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<Middleware> _logger;

    private readonly ILifetimeScope _lifetimeScope;

    public Middleware(RequestDelegate next, ILogger<Middleware> logger, ILifetimeScope lifetimeScope)
    {
        _next = next;
        _logger = logger;
        _lifetimeScope = lifetimeScope;
    }

    // public async Task Invoke(HttpContext context)
    // {
    //     try
    //     {
    //         // Получаем токен из заголовка Authorization
    //         var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    //         if (authHeader == null || !authHeader.StartsWith("Bearer "))
    //         {
    //             await _next(context);
    //             return;
    //         }
    //
    //         var token = authHeader.Substring("Bearer ".Length).Trim();
    //
    //         // Парсим токен и извлекаем jti (уникальный идентификатор токена)
    //         var tokenHandler = new JsonWebTokenHandler();
    //         var jwtToken = tokenHandler.ReadJsonWebToken(token);
    //         var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
    //         var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    //         if (string.IsNullOrEmpty(jti))
    //         {
    //             _logger.LogWarning("Токен не содержит jti.");
    //             await _next(context);
    //             return;
    //         }
    //
    //         using (var scope = _lifetimeScope.BeginLifetimeScope())
    //         {
    //             var apiKeyClaim = context.User.FindAll("apiKey").Select(x => x.Value).ToArray();
    //             var allowedApisClaim = context.User.FindAll("allowedApis").Select(x => x.Value).ToList();
    //             if (apiKeyClaim.Any() && allowedApisClaim.Any())
    //             {
    //                 if (!Guid.TryParse(jti, out Guid userId))
    //                 {
    //                     throw new ArgumentException($"Ошибка Jti не GUID {jti}");
    //                 }
    //
    //                 var requestedApi = context.Request.Path.Value?.TrimStart('/'); // 👈 Получаем запрашиваемый API
    //                 
    //             }
    //             else
    //             {
    //                 throw new ArgumentException($"Доступ закрыт");
    //             }
    //         }
    //
    //         // Передаём запрос дальше по пайплайну
    //         await _next(context);
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "Произошла ошибка во время обработки запроса");
    //         await HandleExceptionAsync(context, ex);
    //     }
    // }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing or invalid Authorization header.");
                return;
            }
        
            var token = authHeader.Substring("Bearer ".Length).Trim();
            
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }
        var tokenHandler = new JwtSecurityTokenHandler();
        using (var scope = _lifetimeScope.BeginLifetimeScope())
        {

            var _setting = scope.Resolve<IJwtSettings>();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // можешь включить true, когда появится exp
                ValidIssuer = _setting.GetJwtIssuer,
                ValidAudience = _setting.GetJwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.GetJwtSecretKey))
            }, out var validatedToken);

            // Если токен валидный — передаём дальше
            context.User = principal;
            // var result = tokenHandler.ValidateToken(token, new TokenValidationParameters
            // {
            //     ValidateIssuer = true,
            //     ValidateAudience = true,
            //     //ValidateLifetime = true,
            //     ValidateIssuerSigningKey = true,
            //     ValidIssuer = _setting.GetJwtIssuer,
            //     ValidAudience = _setting.GetJwtAudience,
            //     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.GetJwtSecretKey))
            // });
            await _next(context);
          
        }

      
        
       

            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка во время обработки запроса");
                await HandleExceptionAsync(context, ex);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка проверки токена");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Token validation failed.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        object response;
        context.Response.ContentType = "application/json";
        
        if (ex is KeyNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

             response = new
            {
                error = ex.Message,
                details = "",
                tracer = "",
                innerexeption = ""
            };
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        
        var formattedTrace = string.Join(" ::: ", ex.StackTrace
            .Split('\n')
            .Where(line => line.Contains(":line")) // Оставляем только строки с номерами строк
            .Select(line => line.Trim()));
      //  formattedTrace = formattedTrace.Replace(" ::: ", Environment.NewLine);
         response = new
        {
            error = "Внутренняя ошибка сервера",
            details = ex.Message,
            tracer = formattedTrace,
            innerexeption = ex.InnerException != null ? ex.InnerException.ToString() : ""
        };
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}