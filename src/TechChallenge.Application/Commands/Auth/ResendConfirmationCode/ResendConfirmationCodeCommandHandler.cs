using MediatR;
using TechChallenge.Application.Contracts.Auth;
namespace TechChallenge.Application.Commands.Auth.ResendConfirmationCode;

public class ResendConfirmationCodeCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ResendConfirmationCodeCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(ResendConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ResendConfirmationCodeAsync(
            request.Email,
            cancellationToken
        );
    }
}
