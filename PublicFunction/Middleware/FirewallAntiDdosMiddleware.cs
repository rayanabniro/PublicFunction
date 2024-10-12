using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class FirewallAntiDdosMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<FirewallAntiDdosMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, DateTime> _blockedIps = new ConcurrentDictionary<string, DateTime>();
        private static readonly ConcurrentDictionary<string, int> _requestCounts = new ConcurrentDictionary<string, int>();
        private readonly int _requestLimit = 100; // Max requests per IP per minute
        private readonly int _timeWindowInMinutes = 1; // Time window for requests
        private readonly string[] _blacklistIps = new string[] { "192.168.1.100", "203.0.113.5" }; // Example blocked IPs

        public FirewallAntiDdosMiddleware(RequestDelegate next, ILogger<FirewallAntiDdosMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string clientIp = context.Connection.RemoteIpAddress?.ToString();

            if (IsBlacklisted(clientIp))
            {
                _logger.LogWarning($"Blocked request from blacklisted IP: {clientIp}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: Your IP is blacklisted.");
                return;
            }

            // Rate limiting based on IP
            if (!RateLimitRequest(clientIp))
            {
                _logger.LogWarning($"Rate limit exceeded for IP: {clientIp}");
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
                return;
            }

            // Continue processing the request
            await _next(context);
        }

        private bool IsBlacklisted(string ip)
        {
            // Check if the IP is in the blacklisted IP array
            return _blacklistIps.Contains(ip);
        }

        private bool RateLimitRequest(string ip)
        {
            var now = DateTime.UtcNow;

            // Increment the request count for the IP
            var requestCount = _requestCounts.AddOrUpdate(ip, 1, (key, oldValue) => oldValue + 1);

            // If the IP has exceeded the rate limit, reject the request
            if (requestCount > _requestLimit)
            {
                return false;
            }

            // Reset the request count after the time window expires
            if (!_blockedIps.ContainsKey(ip) || (now - _blockedIps[ip]).TotalMinutes > _timeWindowInMinutes)
            {
                _blockedIps[ip] = now;
                _requestCounts[ip] = 0;
            }

            return true;
        }
    }

    public static class FirewallAntiDdosMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirewallAntiDdos(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirewallAntiDdosMiddleware>();
        }
    }
}
