using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.ConfirmSignUp;

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
        catch (InvalidConfirmationCodeException)
        {
            return Result.Failure(DomainErrors.Authentication.InvalidConfirmationCode);
        }
        catch (UserNotFoundException)
        {
            return Result.Failure(DomainErrors.Authentication.UserNotFound);
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure(
                Error.Failure("ConfirmSignUp.Failed", $"Failed to confirm sign up: {ex.Message}"));
        }
    }
}
