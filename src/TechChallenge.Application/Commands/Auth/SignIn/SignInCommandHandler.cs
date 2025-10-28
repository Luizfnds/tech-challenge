using MediatR;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Contracts.Auth.Responses;

namespace TechChallenge.Application.Commands.Auth.SignIn;

public class SignInCommandHandler(IAuthenticationService authenticationService) : IRequestHandler<SignInCommand, Token>
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Token> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var token = await _authenticationService.SignInAsync(
            request.Email,
            request.Password,
            cancellationToken
        );

        return token;
    }
}
