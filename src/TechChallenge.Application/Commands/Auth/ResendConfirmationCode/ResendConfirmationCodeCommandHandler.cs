using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.ResendConfirmationCode;

public class ResendConfirmationCodeCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResendConfirmationCodeCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(ResendConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.ResendConfirmationCodeAsync(
                request.Email,
                cancellationToken
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("User does not exist") ||
                ex.Message.Contains("Username/client id combination not found"))
            {
                return Result.Failure(DomainErrors.Authentication.UserNotFound);
            }

            if (ex.Message.Contains("Attempt limit exceeded") ||
                ex.Message.Contains("LimitExceededException"))
            {
                return Result.Failure(
                    Error.Failure("ResendCode.LimitExceeded", "Too many attempts. Please try again later."));
            }

            return Result.Failure(
                Error.Failure("ResendCode.Failed", $"Failed to resend confirmation code: {ex.Message}"));
        }
    }
}
