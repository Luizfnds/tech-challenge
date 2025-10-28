using TechChallenge.Application.Contracts.Auth.Responses;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Contracts.Auth;

public interface IAuthenticationService
{
    // Sign Up
    Task<string> SignUpAsync(User user, string password, CancellationToken cancellationToken = default);
    Task ConfirmSignUpAsync(string email, string confirmationCode, CancellationToken cancellationToken = default);
    Task ResendConfirmationCodeAsync(string email, CancellationToken cancellationToken = default);

    // Sign In
    Task<Token> SignInAsync(string email, string password, CancellationToken cancellationToken = default);

    // Password Management
    Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(string email, string resetCode, string newPassword, CancellationToken cancellationToken = default);
    Task ChangePasswordAsync(string accessToken, string oldPassword, string newPassword, CancellationToken cancellationToken = default);

    // User Management
    Task EnableUserAsync(string email, CancellationToken cancellationToken = default);
    Task DisableUserAsync(string email, CancellationToken cancellationToken = default);
}
