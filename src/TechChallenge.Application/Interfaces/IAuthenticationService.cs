using TechChallenge.Application.DTOs.Auth;

namespace TechChallenge.Application.Interfaces;

public interface IAuthenticationService
{
    Task<string> SignUpAsync(string email, string password, string name, CancellationToken cancellationToken = default);
    Task<TokenResponse> SignInAsync(string email, string password, CancellationToken cancellationToken = default);
    Task ConfirmSignUpAsync(string email, string confirmationCode, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetUserAsync(string email, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(string email, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}
