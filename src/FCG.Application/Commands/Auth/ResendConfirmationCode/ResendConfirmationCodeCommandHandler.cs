using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.ResendConfirmationCode;

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
        catch (UserNotFoundException)
        {
            return Result.Failure(DomainErrors.Authentication.UserNotFound);
        }
        catch (LimitExceededException ex)
        {
            return Result.Failure(
                Error.Failure("ResendCode.LimitExceeded", ex.Message));
        }
        catch (InvalidConfirmationCodeException ex)
        {
            return Result.Failure(
                Error.Failure("ResendCode.Failed", ex.Message));
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure(
                Error.Failure("ResendCode.Failed", $"Failed to resend confirmation code: {ex.Message}"));
        }
    }
}
