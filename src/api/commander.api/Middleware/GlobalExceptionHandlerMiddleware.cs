using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commander.api.Middleware;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IHostEnvironment environment)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;
    private readonly IHostEnvironment _environment = environment;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case BadHttpRequestException badRequestException:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Detail = badRequestException.Message;
                break;

            case KeyNotFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource Not Found";
                problemDetails.Detail = exception.Message;
                break;

            case UnauthorizedAccessException:
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "Authentication is required";
                break;

            case DbUpdateException dbUpdateException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Database Conflict";
                problemDetails.Detail = "A database constraint was violated";
                // Don't expose internal errors in production                
                if (_environment.IsDevelopment())
                {
                    problemDetails.Detail = dbUpdateException.Message;
                }
                break;

            default:
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = _environment.IsDevelopment()
                  ? exception.Message
                  : "An error occurred processing your request";
                break;
        }
        context.Response.StatusCode = problemDetails.Status.Value;

        // Add stack trace in development        
        if (_environment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
            problemDetails.Extensions["innerException"] = exception.InnerException?.Message;
        }
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}