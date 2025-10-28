using MediatR;
using TechChallenge.Application.Contracts.Auth;
namespace TechChallenge.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ForgotPasswordAsync(
            request.Email,
            cancellationToken
        );
    }
}
