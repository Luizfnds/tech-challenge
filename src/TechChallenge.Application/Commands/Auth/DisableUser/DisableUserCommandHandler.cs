using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.DisableUser;

public class DisableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<DisableUserCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.DisableUserAsync(
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
                Error.Failure("DisableUser.Failed", $"Failed to disable user: {ex.Message}"));
        }
    }
}
