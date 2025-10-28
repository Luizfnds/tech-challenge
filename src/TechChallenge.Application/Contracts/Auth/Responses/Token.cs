namespace TechChallenge.Application.Contracts.Auth.Responses;

public record Token(string AccessToken, string IdToken, string RefreshToken, int ExpiresIn);
