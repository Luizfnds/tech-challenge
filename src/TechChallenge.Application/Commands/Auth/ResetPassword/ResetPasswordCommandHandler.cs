using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.ResetPasswordAsync(
                request.Email,
                request.ResetCode,
                request.NewPassword,
                cancellationToken
            );

            return Result.Success();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Invalid verification code") || 
                ex.Message.Contains("Code mismatch") ||
                ex.Message.Contains("Attempt limit exceeded"))
            {
                return Result.Failure(DomainErrors.Authentication.InvalidConfirmationCode);
            }

            if (ex.Message.Contains("Password does not conform") ||
                ex.Message.Contains("Password policy"))
            {
                return Result.Failure(DomainErrors.Authentication.WeakPassword);
            }

            return Result.Failure(DomainErrors.Authentication.PasswordResetFailed);
        }
    }
}
