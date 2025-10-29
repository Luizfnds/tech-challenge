using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;

namespace TechChallenge.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.ForgotPasswordAsync(
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

            return Result.Failure(
                Error.Failure("ForgotPassword.Failed", $"Failed to initiate password reset: {ex.Message}"));
        }
    }
}
