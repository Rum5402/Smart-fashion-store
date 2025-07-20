using Fashion.Contract.DTOs.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Text.Json;

namespace Fashion.Api.Middlewares
{
    /// <summary>
    /// Global exception handler middleware for consistent error handling across the application
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

            var response = new ApiResponse
            {
                Success = false,
                Error = "An unexpected error occurred",
                Details = httpContext.RequestServices.GetService<IWebHostEnvironment>()?.IsDevelopment() == true ? exception.Message : null
            };

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);
            return true;
        }
    }
} 