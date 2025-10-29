using Microsoft.AspNetCore.Mvc;
using TechChallenge.Application.Common.Models;

namespace TechChallenge.API.Extensions;

/// <summary>
/// Extensões para converter Result em IActionResult
/// </summary>
public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
            return new OkResult();

        return result.Error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.NotFound => new NotFoundObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Conflict => new ConflictObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Forbidden => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            })
            { StatusCode = StatusCodes.Status403Forbidden },
            _ => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            })
            { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        return result.Error.Type switch
        {
            ErrorType.Validation => new BadRequestObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.NotFound => new NotFoundObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Conflict => new ConflictObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            }),
            ErrorType.Forbidden => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            })
            { StatusCode = StatusCodes.Status403Forbidden },
            _ => new ObjectResult(new
            {
                error = result.Error.Code,
                message = result.Error.Message
            })
            { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }

    public static IActionResult ToCreatedResult<T>(this Result<T> result, string actionName, object? routeValues = null)
    {
        if (result.IsSuccess)
            return new CreatedAtActionResult(actionName, null, routeValues, result.Value);

        return result.ToActionResult();
    }
}
