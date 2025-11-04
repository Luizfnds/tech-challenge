using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.ChangePassword;

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
        catch (InvalidCredentialsException)
        {
            return Result.Failure(ApplicationErrors.Authentication.InvalidCredentials);
        }
        catch (InvalidTokenException)
        {
            return Result.Failure(ApplicationErrors.Authentication.InvalidToken);
        }
        catch (InvalidPasswordException)
        {
            return Result.Failure(ApplicationErrors.Authentication.WeakPassword);
        }
        catch (LimitExceededException ex)
        {
            return Result.Failure(Error.Failure("ChangePassword.TooManyAttempts", ex.Message));
        }
        catch (PasswordResetFailedException ex)
        {
            return Result.Failure(
                Error.Failure("ChangePassword.Failed", ex.Message));
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure(
                Error.Failure("ChangePassword.Failed", $"Failed to change password: {ex.Message}"));
        }
    }
}
