using MediatR;
using TechChallenge.Application.Contracts.Auth;
namespace TechChallenge.Application.Commands.Auth.EnableUser;

public class EnableUserCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<EnableUserCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(EnableUserCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.EnableUserAsync(
            request.Email,
            cancellationToken
        );
    }
}
