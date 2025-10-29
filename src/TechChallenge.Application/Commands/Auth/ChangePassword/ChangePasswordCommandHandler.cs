using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.ChangePassword;

public class ChangePasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.ChangePasswordAsync(
                request.AccessToken,
                request.OldPassword,
                request.NewPassword,
                cancellationToken
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Incorrect username or password") ||
                ex.Message.Contains("NotAuthorizedException"))
            {
                return Result.Failure(DomainErrors.Authentication.InvalidCredentials);
            }

            if (ex.Message.Contains("Access Token has expired") ||
                ex.Message.Contains("Invalid Access Token"))
            {
                return Result.Failure(DomainErrors.Authentication.InvalidToken);
            }

            if (ex.Message.Contains("Password does not conform") ||
                ex.Message.Contains("Password policy"))
            {
                return Result.Failure(DomainErrors.Authentication.WeakPassword);
            }

            return Result.Failure(
                Error.Failure("ChangePassword.Failed", $"Failed to change password: {ex.Message}"));
        }
    }
}
