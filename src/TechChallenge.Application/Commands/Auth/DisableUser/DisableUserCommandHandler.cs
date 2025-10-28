using MediatR;
using TechChallenge.Application.Contracts.Auth;
namespace TechChallenge.Application.Commands.Auth.DisableUser;

public class DisableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<DisableUserCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(DisableUserCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.DisableUserAsync(
            request.Email,
            cancellationToken
        );
    }
}
