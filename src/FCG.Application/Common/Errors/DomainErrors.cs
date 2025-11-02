using FCG.Application.Common.Models;

namespace FCG.Application.Common.Errors;

/// <summary>
/// Catálogo centralizado de erros do domínio
/// </summary>
public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound(Guid userId) => Error.NotFound(
            "User.NotFound",
            $"User with ID '{userId}' was not found");

        public static Error EmailAlreadyExists(string email) => Error.Conflict(
            "User.EmailAlreadyExists",
            $"User with email '{email}' already exists");

        public static Error InvalidEmail(string email) => Error.Validation(
            "User.InvalidEmail",
            $"Email '{email}' is not valid");

        public static Error InvalidName => Error.Validation(
            "User.InvalidName",
            "User name must be between 3 and 100 characters");

        public static Error CreationFailed => Error.Failure(
            "User.CreationFailed",
            "Failed to create user");
    }

    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Unauthorized(
            "Authentication.InvalidCredentials",
            "Invalid email or password");

        public static Error EmailNotConfirmed => Error.Unauthorized(
            "Authentication.EmailNotConfirmed",
            "Email address is not confirmed");

        public static Error InvalidConfirmationCode => Error.Validation(
            "Authentication.InvalidConfirmationCode",
            "Invalid or expired confirmation code");

        public static Error UserNotFound => Error.NotFound(
            "Authentication.UserNotFound",
            "User not found");

        public static Error UserDisabled => Error.Forbidden(
            "Authentication.UserDisabled",
            "User account is disabled");

        public static Error PasswordResetFailed => Error.Failure(
            "Authentication.PasswordResetFailed",
            "Failed to reset password");

        public static Error InvalidToken => Error.Unauthorized(
            "Authentication.InvalidToken",
            "Invalid or expired token");

        public static Error WeakPassword => Error.Validation(
            "Authentication.WeakPassword",
            "Password does not meet security requirements");
    }

    public static class General
    {
        public static Error UnexpectedError => Error.Failure(
            "General.UnexpectedError",
            "An unexpected error occurred");

        public static Error ValidationError(string message) => Error.Validation(
            "General.ValidationError",
            message);

        public static Error ServerError => Error.Failure(
            "General.ServerError",
            "An internal server error occurred");
    }
}
