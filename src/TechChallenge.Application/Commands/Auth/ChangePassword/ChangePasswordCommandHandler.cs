using MediatR;
using TechChallenge.Application.Contracts.Auth;

namespace TechChallenge.Application.Commands.Auth.ChangePassword;

public class ChangePasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ChangePasswordCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ChangePasswordAsync(
            request.AccessToken,
            request.OldPassword,
            request.NewPassword,
            cancellationToken
        );
    }
}
