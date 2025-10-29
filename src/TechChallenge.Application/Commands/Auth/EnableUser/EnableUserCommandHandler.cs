using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.EnableUser;

public class EnableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<EnableUserCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(EnableUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.EnableUserAsync(
                request.Email,
                cancellationToken
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("User does not exist") ||
                ex.Message.Contains("UserNotFoundException"))
            {
                return Result.Failure(DomainErrors.Authentication.UserNotFound);
            }

            return Result.Failure(
                Error.Failure("EnableUser.Failed", $"Failed to enable user: {ex.Message}"));
        }
    }
}
