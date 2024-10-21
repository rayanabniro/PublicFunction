using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class ApiVersioningMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiVersioningMiddleware> _logger;

        public ApiVersioningMiddleware(RequestDelegate next, ILogger<ApiVersioningMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract API version from the URL (e.g., /api/v1/resource or /api/v2/resource)
            var pathSegments = context.Request.Path.Value.Split('/');
            var versionSegment = pathSegments.FirstOrDefault(segment => segment.StartsWith("v", StringComparison.OrdinalIgnoreCase));

            if (versionSegment != null)
            {
                // Extract the version number, e.g., "v1" becomes "1"
                var version = versionSegment.TrimStart('v');
                _logger.LogInformation("API version requested: v{Version}", version);

                // Set the version information in HttpContext
                context.Items["ApiVersion"] = version;
            }
            else
            {
                // If no version is provided in the URL, use a default version (e.g., "1")
                _logger.LogInformation("No API version specified. Defaulting to version 1.");
                context.Items["ApiVersion"] = "1";
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    public static class ApiVersioningMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiVersioning(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiVersioningMiddleware>();
        }
    }
}
