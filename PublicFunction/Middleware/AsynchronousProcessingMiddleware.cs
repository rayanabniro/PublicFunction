using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class AsynchronousProcessingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AsynchronousProcessingMiddleware> _logger;

        public AsynchronousProcessingMiddleware(RequestDelegate next, ILogger<AsynchronousProcessingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the start of the asynchronous task
            _logger.LogInformation("Starting asynchronous processing for request: {RequestPath}", context.Request.Path);

            // Simulate a long-running task (e.g., a 5-second delay to represent a time-consuming operation)
            await SimulateLongRunningTaskAsync();

            // Log that the asynchronous task has completed
            _logger.LogInformation("Asynchronous processing completed for request: {RequestPath}", context.Request.Path);

            // Continue with the next middleware or request processing
            await _next(context);
        }

        private async Task SimulateLongRunningTaskAsync()
        {
            // Simulate a long-running task asynchronously (e.g., 5 seconds)
            await Task.Delay(5000); // Simulate delay (5 seconds)
        }
    }

    public static class AsynchronousProcessingMiddlewareExtensions
    {
        public static IApplicationBuilder UseAsynchronousProcessing(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AsynchronousProcessingMiddleware>();
        }
    }
}
