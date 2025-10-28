using MediatR;
using TechChallenge.Application.Contracts.Auth;using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Contracts.Repositories;

namespace TechChallenge.Application.Commands.Auth.SignUp;

public class SignUpCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService) : IRequestHandler<SignUpCommand, Guid>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Guid> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already exists");

        var user = User.CreateUser(
            request.Name,
            request.Email
        );

        var cognitoUserId = await _authenticationService.SignUpAsync(
            user,
            request.Password,
            cancellationToken
        );

        user.SetCognitoUserId(cognitoUserId);

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}