namespace TechChallenge.Application.Contracts.Auth.Responses;

public record Account(
    string Username,
    string Email,
    bool EmailVerified,
    string Status,
    DateTime CreatedDate
);
