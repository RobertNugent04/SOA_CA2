using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace SOA_CA2.Middleware
{
    /// <summary>
    /// Middleware to handle exceptions and provide consistent error responses.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Intercept requests and handle exceptions globally.
        /// </summary>
        /// <param name="context">HTTP context for the current request.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Process the next middleware in the pipeline.
                await _next(context);
            }
            catch (Exception ex)
            {
                // Handle any uncaught exceptions.
                _logger.LogError(ex, "An unexpected error occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Customize error response here.
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            int statusCode = exception switch
            {
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            string result = JsonSerializer.Serialize(new
            {
                error = exception.Message,
                statusCode = statusCode
            });

            response.StatusCode = statusCode;
            return response.WriteAsync(result);
        }
    }
}
