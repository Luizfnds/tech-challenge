using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using TechChallenge.Infrastructure.AWS.Cognito.Configuration;
using System.Security.Cryptography;
using System.Text;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.DTOs.Auth;

namespace TechChallenge.Infrastructure.AWS.Cognito.Services;

public class CognitoService : IAuthenticationService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly CognitoSettings _settings;

    public CognitoService(
        IAmazonCognitoIdentityProvider cognitoClient,
        IOptions<CognitoSettings> settings)
    {
        _cognitoClient = cognitoClient;
        _settings = settings.Value;
    }

    public async Task<string> SignUpAsync(string email, string password, string name, CancellationToken cancellationToken = default)
    {
        var request = new SignUpRequest
        {
            ClientId = _settings.ClientId,
            Username = email,
            Password = password,
            SecretHash = GenerateSecretHash(email),
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "email", Value = email },
                new AttributeType { Name = "name", Value = name }
            }
        };

        var response = await _cognitoClient.SignUpAsync(request, cancellationToken);
        return response.UserSub;
    }

    public async Task<TokenResponse> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var request = new AdminInitiateAuthRequest
        {
            UserPoolId = _settings.UserPoolId,
            ClientId = _settings.ClientId,
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password },
                { "SECRET_HASH", GenerateSecretHash(email) }
            }
        };

        var response = await _cognitoClient.AdminInitiateAuthAsync(request, cancellationToken);

        return new TokenResponse
        {
            AccessToken = response.AuthenticationResult.AccessToken,
            IdToken = response.AuthenticationResult.IdToken,
            RefreshToken = response.AuthenticationResult.RefreshToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn
        };
    }

    public async Task ConfirmSignUpAsync(string email, string confirmationCode, CancellationToken cancellationToken = default)
    {
        var request = new ConfirmSignUpRequest
        {
            ClientId = _settings.ClientId,
            Username = email,
            ConfirmationCode = confirmationCode,
            SecretHash = GenerateSecretHash(email)
        };

        await _cognitoClient.ConfirmSignUpAsync(request, cancellationToken);
    }

    public async Task<UserResponse?> GetUserAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new AdminGetUserRequest
            {
                UserPoolId = _settings.UserPoolId,
                Username = email
            };

            var response = await _cognitoClient.AdminGetUserAsync(request, cancellationToken);

            return new UserResponse
            {
                Username = response.Username,
                Email = response.UserAttributes.FirstOrDefault(a => a.Name == "email")?.Value ?? string.Empty,
                EmailVerified = bool.Parse(response.UserAttributes.FirstOrDefault(a => a.Name == "email_verified")?.Value ?? "false"),
                Status = response.UserStatus.Value,
                CreatedDate = response.UserCreateDate
            };
        }
        catch (UserNotFoundException)
        {
            return null;
        }
    }

    public async Task DeleteUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var request = new AdminDeleteUserRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = email
        };

        await _cognitoClient.AdminDeleteUserAsync(request, cancellationToken);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var request = new AdminInitiateAuthRequest
        {
            UserPoolId = _settings.UserPoolId,
            ClientId = _settings.ClientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", refreshToken }
            }
        };

        var response = await _cognitoClient.AdminInitiateAuthAsync(request, cancellationToken);

        return new TokenResponse
        {
            AccessToken = response.AuthenticationResult.AccessToken,
            IdToken = response.AuthenticationResult.IdToken,
            RefreshToken = refreshToken,
            ExpiresIn = response.AuthenticationResult.ExpiresIn
        };
    }

    private string GenerateSecretHash(string username)
    {
        var message = username + _settings.ClientId;
        var keyBytes = Encoding.UTF8.GetBytes(_settings.ClientSecret);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
