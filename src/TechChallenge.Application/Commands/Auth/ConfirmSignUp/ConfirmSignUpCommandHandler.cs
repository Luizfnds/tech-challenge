using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.ConfirmSignUp;

public class ConfirmSignUpCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ConfirmSignUpCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(ConfirmSignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.ConfirmSignUpAsync(
                request.Email,
                request.ConfirmationCode,
                cancellationToken
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Invalid verification code") || 
                ex.Message.Contains("Code mismatch"))
            {
                return Result.Failure(DomainErrors.Authentication.InvalidConfirmationCode);
            }

            if (ex.Message.Contains("User cannot be confirmed"))
            {
                return Result.Failure(DomainErrors.Authentication.UserNotFound);
            }

            return Result.Failure(
                Error.Failure("ConfirmSignUp.Failed", $"Failed to confirm sign up: {ex.Message}"));
        }
    }
}
