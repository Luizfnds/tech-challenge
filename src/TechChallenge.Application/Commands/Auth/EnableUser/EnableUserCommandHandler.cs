using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Common.Models;
using TechChallenge.Application.Common.Errors;
using TechChallenge.Application.Common.Exceptions;

namespace TechChallenge.Application.Commands.Auth.EnableUser;

public class EnableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<EnableUserCommand, Result>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result> Handle(EnableUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _authenticationService.EnableUserAsync(
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
                Error.Failure("EnableUser.Failed", ex.Message));
        }
    }
}
