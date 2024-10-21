using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class LoadBalancingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoadBalancingMiddleware> _logger;
        private readonly List<string> _backendServers;
        private static int _currentServerIndex = 0;
        private readonly HttpClient _httpClient;

        public LoadBalancingMiddleware(RequestDelegate next, ILogger<LoadBalancingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _httpClient = new HttpClient();

            // A list of backend servers (URLs or IP addresses) for load balancing
            _backendServers = new List<string>
        {
            "http://localhost:5001", // Server 1
            "http://localhost:5002", // Server 2
            "http://localhost:5003"  // Server 3
        };
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get the next server using round-robin strategy
            var serverUrl = GetNextServer();

            _logger.LogInformation("Routing request to: {ServerUrl}", serverUrl);

            // Build the target request
            var requestMessage = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = new Uri($"{serverUrl}{context.Request.Path}{context.Request.QueryString}")
            };

            // Copy headers and body from the original request
            foreach (var header in context.Request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            // Forward the request to the backend server
            var responseMessage = await _httpClient.SendAsync(requestMessage);

            // Copy the response from the backend server to the client
            context.Response.StatusCode = (int)responseMessage.StatusCode;
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            await responseMessage.Content.CopyToAsync(context.Response.Body);
        }

        private string GetNextServer()
        {
            // Get the next server in the list (round-robin)
            var server = _backendServers[_currentServerIndex];
            _currentServerIndex = (_currentServerIndex + 1) % _backendServers.Count;
            return server;
        }
    }

    public static class LoadBalancingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoadBalancing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoadBalancingMiddleware>();
        }
    }
}
