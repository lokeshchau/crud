using System.Net;
using System.Text.Json;
using CrudOp.Exceptions;
using CrudOp.Models;

namespace CrudOp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                NotFoundException notFoundEx => (StatusCodes.Status404NotFound, notFoundEx.Message),
                KeyNotFoundException keyNotFoundEx => (StatusCodes.Status404NotFound, keyNotFoundEx.Message),
                BadRequestException badRequestEx => (StatusCodes.Status400BadRequest, badRequestEx.Message),
                ArgumentException argEx => (StatusCodes.Status400BadRequest, argEx.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.")
            };

            if (statusCode >= 500)
            {
                _logger.LogError(exception, "Unhandled Exception: {Message}", exception.Message);
            }
            else
            {
                _logger.LogWarning("Handled Exception ({StatusCode}): {Message}", statusCode, exception.Message);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorResponse = new ErrorResponse
            {
                StatusCode = statusCode,
                Message = message,
                Details = _env.IsDevelopment() ? exception.StackTrace : null,
                Timestamp = DateTime.UtcNow
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, jsonOptions));
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
