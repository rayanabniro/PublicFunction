using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private readonly IMemoryCache _cache;
        private readonly int _requestLimit = 100; // Max requests per minute
        private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); // 1 minute window

        public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger, IMemoryCache cache)
        {
            _next = next;
            _logger = logger;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"RequestCount:{ipAddress}";

            var currentRequestCount = _cache.Get<int>(cacheKey);

            if (currentRequestCount >= _requestLimit)
            {
                // If request limit is exceeded
                _logger.LogWarning("Rate limit exceeded for IP {IpAddress}.", ipAddress);
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            // Increment the request count in memory cache
            _cache.Set(cacheKey, currentRequestCount + 1, _timeWindow);

            // Continue processing the request
            await _next(context);
        }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>();
        }
    }
}
