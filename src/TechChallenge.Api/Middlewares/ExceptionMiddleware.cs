using System.Net;
using System.Text.Json;
using TechChallenge.Domain.Exceptions;
using TechChallenge.Application.Common.Exceptions;

namespace TechChallenge.API.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An internal server error occurred";
        var errors = new List<string>();

        switch (exception)
        {
            // Authentication Exceptions
            case InvalidCredentialsException:
            case InvalidTokenException:
                statusCode = HttpStatusCode.Unauthorized;
                message = "Authentication failed";
                errors.Add(exception.Message);
                break;

            case UserDisabledException:
                statusCode = HttpStatusCode.Forbidden;
                message = "User account is disabled";
                errors.Add(exception.Message);
                break;

            case EmailNotConfirmedException:
                statusCode = HttpStatusCode.Forbidden;
                message = "Email not confirmed";
                errors.Add(exception.Message);
                break;

            case UserNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "User not found";
                errors.Add(exception.Message);
                break;

            case UserAlreadyExistsException:
                statusCode = HttpStatusCode.Conflict;
                message = "User already exists";
                errors.Add(exception.Message);
                break;

            case InvalidPasswordException:
            case InvalidConfirmationCodeException:
            case PasswordResetFailedException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid request";
                errors.Add(exception.Message);
                break;

            case LimitExceededException:
                statusCode = HttpStatusCode.TooManyRequests;
                message = "Rate limit exceeded";
                errors.Add(exception.Message);
                break;

            case Application.Common.Exceptions.AuthenticationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Authentication error";
                errors.Add(exception.Message);
                break;

            // Domain Exceptions
            case DomainException domainException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Domain validation error";
                errors.Add(domainException.Message);
                break;

            // General Exceptions
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
                errors.Add(exception.Message);
                break;

            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid operation";
                errors.Add(exception.Message);
                break;

            case ArgumentException:
                statusCode = HttpStatusCode.BadRequest;
                message = "Invalid argument";
                errors.Add(exception.Message);
                break;

            default:
                errors.Add("An unexpected error occurred");
                break;
        }

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Message = message,
            Errors = errors
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}
