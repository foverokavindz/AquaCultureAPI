using AquaCulture.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AquaCulture.API.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            switch (exception)
            {
                case KeyNotFoundException:
                    statusCode = 404;
                    message = exception.Message;
                    break;

                case ArgumentException:
                    statusCode = 400;
                    message = exception.Message;
                    break;

                default:
                    statusCode = 500;
                    message = "An unexpected error occurred";
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                message = message,
                data = (object?)null,
                error = exception.Message,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}