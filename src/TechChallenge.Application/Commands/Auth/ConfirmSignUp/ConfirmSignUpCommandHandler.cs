using MediatR;
using TechChallenge.Application.Contracts.Auth;

namespace TechChallenge.Application.Commands.Auth.ConfirmSignUp;

public class ConfirmSignUpCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<ConfirmSignUpCommand>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task Handle(ConfirmSignUpCommand request, CancellationToken cancellationToken)
    {
        await _authenticationService.ConfirmSignUpAsync(
            request.Email,
            request.ConfirmationCode,
            cancellationToken
        );
    }
}
