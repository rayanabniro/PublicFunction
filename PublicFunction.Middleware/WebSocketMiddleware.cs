using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublicFunction.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<WebSocketMiddleware> _logger;

        public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is asking for a WebSocket connection
            if (context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                _logger.LogInformation("WebSocket connection established.");

                // Handle WebSocket communication
                await HandleWebSocketCommunication(webSocket);
            }
            else
            {
                // If it's not a WebSocket request, pass to the next middleware
                await _next(context);
            }
        }

        private async Task HandleWebSocketCommunication(System.Net.WebSockets.WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4]; // Buffer size for receiving data
            WebSocketReceiveResult result = null;

            try
            {
                // Keep receiving messages from the WebSocket connection
                while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        _logger.LogInformation("Received message: {Message}", message);

                        // Echo the received message back to the client
                        await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Echo: {message}")), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                    else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                    {
                        _logger.LogInformation("WebSocket connection closed.");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while handling WebSocket communication: {Error}", ex.Message);
            }
            finally
            {
                // Close the WebSocket connection
                if (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
    }

    public static class WebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebSocketMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
