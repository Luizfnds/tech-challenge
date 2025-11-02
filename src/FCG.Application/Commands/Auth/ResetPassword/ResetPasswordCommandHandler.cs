using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.ResetPassword;

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
        catch (InvalidConfirmationCodeException)
        {
            return Result.Failure(DomainErrors.Authentication.InvalidConfirmationCode);
        }
        catch (InvalidPasswordException)
        {
            return Result.Failure(DomainErrors.Authentication.WeakPassword);
        }
        catch (UserNotFoundException)
        {
            return Result.Failure(DomainErrors.Authentication.UserNotFound);
        }
        catch (PasswordResetFailedException)
        {
            return Result.Failure(DomainErrors.Authentication.PasswordResetFailed);
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure(
                Error.Failure("ResetPassword.Failed", $"Failed to reset password: {ex.Message}"));
        }
    }
}
