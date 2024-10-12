using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class SignalRMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SignalRMiddleware> _logger;

        public SignalRMiddleware(RequestDelegate next, ILogger<SignalRMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log each request that passes through the middleware
            _logger.LogInformation("Processing SignalR request: {RequestPath}", context.Request.Path);

            // Proceed with the next middleware in the pipeline
            await _next(context);
        }
    }

    public static class SignalRMiddlewareExtensions
    {
        public static IApplicationBuilder UseSignalRMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SignalRMiddleware>();
        }
    }
}
