using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;
using TechChallenge.Application.Common.Exceptions;

namespace TechChallenge.Application.Commands.Auth.DisableUser;

public class DisableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<DisableUserCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.DisableUserAsync(
                request.Email,
                cancellationToken
            );

            return Result.Success();
        }
        catch (UserNotFoundException)
        {
            return Result.Failure(DomainErrors.Authentication.UserNotFound);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure(
                Error.Failure("DisableUser.Failed", ex.Message));
        }
    }
}
