using MediatR;
using TechChallenge.Application.Contracts.Auth;
namespace TechChallenge.Application.Commands.Auth.ResetPassword;

public class ResetPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResetPasswordCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ResetPasswordAsync(
            request.Email,
            request.ResetCode,
            request.NewPassword,
            cancellationToken
        );
    }
}
