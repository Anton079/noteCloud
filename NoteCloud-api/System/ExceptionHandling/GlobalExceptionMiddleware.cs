using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.System.ExceptionHandling
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "Handled application exception: {Message}", ex.Message);
                await WriteProblem(context, ex.StatusCode, ex.ErrorCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblem(context, 500, "server_error", "Internal server error.");
            }
        }

        private static async Task WriteProblem(HttpContext context, int statusCode, string code, string message)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var payload = new
            {
                type = "about:blank",
                title = code,
                status = statusCode,
                detail = message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
