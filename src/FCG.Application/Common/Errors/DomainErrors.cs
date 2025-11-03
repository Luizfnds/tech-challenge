using FCG.Application.Common.Models;

namespace FCG.Application.Common.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error NotFound(Guid userId) => Error.NotFound(
            "User.NotFound",
            $"User with ID '{userId}' was not found");

        public static Error NotFound(string email) => Error.NotFound(
            "User.NotFound",
            $"User with email '{email}' was not found");

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

    public static class Game
    {
        public static Error NotFound(Guid gameId) => Error.NotFound(
            "Game.NotFound",
            $"Game with ID '{gameId}' was not found");

        public static Error InvalidTitle => Error.Validation(
            "Game.InvalidTitle",
            "Game title must be between 3 and 200 characters");

        public static Error InvalidPrice => Error.Validation(
            "Game.InvalidPrice",
            "Game price must be greater than or equal to zero");

        public static Error CreationFailed => Error.Failure(
            "Game.CreationFailed",
            "Failed to create game");

        public static Error UpdateFailed => Error.Failure(
            "Game.UpdateFailed",
            "Failed to update game");

        public static Error DeleteFailed => Error.Failure(
            "Game.DeleteFailed",
            "Failed to delete game");

        public static Error AlreadyInactive => Error.Validation(
            "Game.AlreadyInactive",
            "Game is already inactive");

        public static Error AlreadyActive => Error.Validation(
            "Game.AlreadyActive",
            "Game is already active");
    }

    public static class UserGame
    {
        public static Error NotFound(Guid userGameId) => Error.NotFound(
            "UserGame.NotFound",
            $"User game with ID '{userGameId}' was not found");

        public static Error GameNotActive => Error.Validation(
            "UserGame.GameNotActive",
            "Game is not active and cannot be purchased");

        public static Error AlreadyOwned => Error.Conflict(
            "UserGame.AlreadyOwned",
            "User already owns this game");

        public static Error PurchaseFailed => Error.Failure(
            "UserGame.PurchaseFailed",
            "Failed to purchase game");

        public static Error InvalidPurchasePrice => Error.Validation(
            "UserGame.InvalidPurchasePrice",
            "Purchase price must be greater than or equal to zero");
    }

    public static class Promotion
    {
        public static Error NotFound(Guid promotionId) => Error.NotFound(
            "Promotion.NotFound",
            $"Promotion with ID '{promotionId}' was not found");

        public static Error GameNotFound => Error.NotFound(
            "Promotion.GameNotFound",
            "Game not found for this promotion");

        public static Error GameAlreadyHasPromotion => Error.Conflict(
            "Promotion.GameAlreadyHasPromotion",
            "This game already has an active promotion");

        public static Error InvalidDiscountPercentage => Error.Validation(
            "Promotion.InvalidDiscountPercentage",
            "Discount percentage must be between 0 and 100");

        public static Error InvalidDateRange => Error.Validation(
            "Promotion.InvalidDateRange",
            "Start date must be before end date and end date must be in the future");

        public static Error CreationFailed => Error.Failure(
            "Promotion.CreationFailed",
            "Failed to create promotion");

        public static Error AlreadyInactive => Error.Validation(
            "Promotion.AlreadyInactive",
            "Promotion is already inactive");
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
