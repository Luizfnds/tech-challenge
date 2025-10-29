using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Contracts.Auth.Responses;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.SignIn;

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
        catch (Exception ex)
        {
            // Tratar erros específicos do Cognito
            if (ex.Message.Contains("Incorrect username or password") || 
                ex.Message.Contains("User does not exist"))
            {
                return Result.Failure<Token>(DomainErrors.Authentication.InvalidCredentials);
            }

            if (ex.Message.Contains("User is disabled"))
            {
                return Result.Failure<Token>(DomainErrors.Authentication.UserDisabled);
            }

            if (ex.Message.Contains("User is not confirmed"))
            {
                return Result.Failure<Token>(DomainErrors.Authentication.EmailNotConfirmed);
            }

            return Result.Failure<Token>(
                Error.Failure("SignIn.Failed", $"Failed to sign in: {ex.Message}"));
        }
    }
}
