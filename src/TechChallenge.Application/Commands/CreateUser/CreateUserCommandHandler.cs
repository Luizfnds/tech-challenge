using MediatR;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;

    public CreateUserCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Check if email already exists
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already exists");

        // Create user entity
        var user = User.Create(
            request.Name,
            request.Email
        );

        // Sign up user in Cognito
        var cognitoUserId = await _authenticationService.SignUpAsync(
            request.Email,
            request.Password,
            request.Name,
            cancellationToken
        );

        // Set Cognito User ID
        user.SetCognitoUserId(cognitoUserId);

        // Save to repository
        await _userRepository.AddAsync(user);

        // Return DTO
        return user.Id;
    }
}