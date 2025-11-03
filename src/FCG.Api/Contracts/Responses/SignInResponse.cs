namespace FCG.API.Contracts.Responses;

public record SignInResponse(
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int ExpiresIn
);
