using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;
using TechChallenge.Application.Common.Exceptions;

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
