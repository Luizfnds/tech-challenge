using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using TechChallenge.Infrastructure.AWS.Cognito.Configuration;
using System.Security.Cryptography;
using System.Text;
using TechChallenge.Domain.Entities;
using TechChallenge.Application.Contracts.Auth;
using TechChallenge.Application.Contracts.Auth.Responses;

namespace TechChallenge.Infrastructure.AWS.Cognito.Services;

public class CognitoService(
    IAmazonCognitoIdentityProvider cognitoClient,
    IOptions<CognitoSettings> settings) : IAuthenticationService
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient = cognitoClient;
    private readonly CognitoSettings _settings = settings.Value;

    public async Task<string> SignUpAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        // 1. Criar usuário no Cognito
        var signUpRequest = new SignUpRequest
        {
            ClientId = _settings.ClientId,
            Username = user.Email,
            Password = password,
            SecretHash = GenerateSecretHash(user.Email),
            UserAttributes =
            [
                new() { Name = "email", Value = user.Email },
                new() { Name = "name", Value = user.Name },
            ]
        };

        var signUpResponse = await _cognitoClient.SignUpAsync(signUpRequest, cancellationToken);

        // 2. Adicionar usuário ao grupo correspondente
        await AddUserToGroupAsync(user.Email, user.Role.Name, cancellationToken);

        return signUpResponse.UserSub;
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

    public async Task ResendConfirmationCodeAsync(string email, CancellationToken cancellationToken = default)
    {
        var request = new ResendConfirmationCodeRequest
        {
            ClientId = _settings.ClientId,
            Username = email,
            SecretHash = GenerateSecretHash(email)
        };

        await _cognitoClient.ResendConfirmationCodeAsync(request, cancellationToken);
    }

    public async Task<Token> SignInAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var request = new InitiateAuthRequest
        {
            ClientId = _settings.ClientId,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password },
                { "SECRET_HASH", GenerateSecretHash(email) }
            }
        };

        var response = await _cognitoClient.InitiateAuthAsync(request, cancellationToken);

        return TokenFromAuthResult(response.AuthenticationResult);
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
    {
        var request = new ForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            Username = email,
            SecretHash = GenerateSecretHash(email)
        };

        await _cognitoClient.ForgotPasswordAsync(request, cancellationToken);
    }

    public async Task ResetPasswordAsync(string email, string resetCode, string newPassword, CancellationToken cancellationToken = default)
    {
        var request = new ConfirmForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            Username = email,
            ConfirmationCode = resetCode,
            Password = newPassword,
            SecretHash = GenerateSecretHash(email)
        };

        await _cognitoClient.ConfirmForgotPasswordAsync(request, cancellationToken);
    }

    public async Task ChangePasswordAsync(string accessToken, string oldPassword, string newPassword, CancellationToken cancellationToken = default)
    {
        var request = new ChangePasswordRequest
        {
            AccessToken = accessToken,
            PreviousPassword = oldPassword,
            ProposedPassword = newPassword
        };

        await _cognitoClient.ChangePasswordAsync(request, cancellationToken);
    }

    public async Task EnableUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var request = new AdminEnableUserRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = email
        };

        await _cognitoClient.AdminEnableUserAsync(request, cancellationToken);
    }

    public async Task DisableUserAsync(string email, CancellationToken cancellationToken = default)
    {
        var request = new AdminDisableUserRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = email
        };

        await _cognitoClient.AdminDisableUserAsync(request, cancellationToken);
    }

    #region private methods

    private string GenerateSecretHash(string username)
    {
        var message = username + _settings.ClientId;
        var keyBytes = Encoding.UTF8.GetBytes(_settings.ClientSecret);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(messageBytes);
        return Convert.ToBase64String(hashBytes);
    }

    private static Token TokenFromAuthResult(AuthenticationResultType authResult, string? refreshToken = null)
    {
        return new Token(
            AccessToken: authResult.AccessToken,
            IdToken: authResult.IdToken,
            RefreshToken: refreshToken ?? authResult.RefreshToken,
            ExpiresIn: authResult.ExpiresIn
        );
    }

    private async Task AddUserToGroupAsync(string email, string groupName, CancellationToken cancellationToken)
    {
        var request = new AdminAddUserToGroupRequest
        {
            UserPoolId = _settings.UserPoolId,
            Username = email,
            GroupName = groupName
        };

        await _cognitoClient.AdminAddUserToGroupAsync(request, cancellationToken);
    }

    #endregion
}
