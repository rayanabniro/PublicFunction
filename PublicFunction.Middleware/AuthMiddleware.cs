using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;
        private readonly string _secretKey;  // Shared secret key for JWT encoding/decoding
        private readonly string _issuer;     // Issuer for the JWT
        private readonly string _audience;   // Audience for the JWT

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, string secretKey, string issuer, string audience)
        {
            _next = next;
            _logger = logger;
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1. Check for the Authorization header
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                // 2. Validate the JWT Token
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  // Optional: To control the token expiration skew
                };

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    // 3. Attach the user identity
                    context.User = principal;
                }
                catch (SecurityTokenException ex)
                {
                    _logger.LogWarning("Invalid token: {Message}", ex.Message);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }
            }
            else
            {
                // No token provided
                _logger.LogWarning("Authorization header missing or invalid");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Authorization header missing or invalid");
                return;
            }

            // 4. Authorization Check (RBAC or ABAC)
            // Example: Only users with the "admin" role can access this resource
            if (!context.User.IsInRole("admin"))
            {
                _logger.LogWarning("Access denied for user {Username}", context.User.Identity.Name);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden");
                return;
            }

            // Proceed to the next middleware
            await _next(context);
        }
    }

    public static class AuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder, string secretKey, string issuer, string audience)
        {
            return builder.UseMiddleware<AuthMiddleware>(secretKey, issuer, audience);
        }
    }

}
