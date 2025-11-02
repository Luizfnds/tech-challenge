using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Application.Contracts.Auth.Responses;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.SignIn;

public class SignInCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<SignInCommand, Result<Token>>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<Token>> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var token = await _authenticationService.SignInAsync(
                request.Email,
                request.Password,
                cancellationToken
            );

            return Result.Success(token);
        }
        catch (InvalidCredentialsException)
        {
            return Result.Failure<Token>(DomainErrors.Authentication.InvalidCredentials);
        }
        catch (EmailNotConfirmedException)
        {
            return Result.Failure<Token>(DomainErrors.Authentication.EmailNotConfirmed);
        }
        catch (UserDisabledException)
        {
            return Result.Failure<Token>(DomainErrors.Authentication.UserDisabled);
        }
        catch (UserNotFoundException)
        {
            return Result.Failure<Token>(DomainErrors.Authentication.InvalidCredentials); // Não expor se o usuário existe
        }
        catch (LimitExceededException ex)
        {
            return Result.Failure<Token>(Error.Failure("SignIn.TooManyAttempts", ex.Message));
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure<Token>(
                Error.Failure("SignIn.Failed", $"Failed to sign in: {ex.Message}"));
        }
    }
}
