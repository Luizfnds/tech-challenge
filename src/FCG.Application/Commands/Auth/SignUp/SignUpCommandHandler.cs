using MediatR;
using FCG.Application.Contracts.Auth;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Application.Common.Exceptions;

namespace FCG.Application.Commands.Auth.SignUp;

public class SignUpCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService) : IRequestHandler<SignUpCommand, Result<Guid>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<Result<Guid>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Verificar se o email já existe
            if (await _userRepository.EmailExistsAsync(request.Email))
                return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyExists(request.Email));

            // Criar usuário
            var user = User.CreateUser(
                request.Name,
                request.Email
            );

            // Registrar no Cognito
            var cognitoUserId = await _authenticationService.SignUpAsync(
                user,
                request.Password,
                cancellationToken
            );

            user.SetAccountId(cognitoUserId);

            // Salvar no banco de dados
            await _userRepository.AddAsync(user);

            return Result.Success(user.Id);
        }
        catch (UserAlreadyExistsException)
        {
            return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyExists(request.Email));
        }
        catch (InvalidPasswordException ex)
        {
            return Result.Failure<Guid>(Error.Validation("SignUp.InvalidPassword", ex.Message));
        }
        catch (LimitExceededException ex)
        {
            return Result.Failure<Guid>(Error.Failure("SignUp.TooManyAttempts", ex.Message));
        }
        catch (AuthenticationException ex)
        {
            return Result.Failure<Guid>(
                Error.Failure("SignUp.Failed", $"Failed to sign up user: {ex.Message}"));
        }
    }
}