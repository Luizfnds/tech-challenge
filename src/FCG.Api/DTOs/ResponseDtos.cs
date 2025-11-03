using FCG.Domain.Entities;

namespace FCG.API.DTOs;

public class GamePagedResponse
{
    public IEnumerable<Game> Items { get; set; } = new List<Game>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class CreateGameResponse
{
    public Guid GameId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class MessageResponse
{
    public string Message { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class SignUpResponse
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class SignInResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? AccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class UserGamePagedResponse
{
    public IEnumerable<UserGameItemResponse> Items { get; set; } = new List<UserGameItemResponse>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class UserGameItemResponse
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string GameTitle { get; set; } = string.Empty;
    public string GameDescription { get; set; } = string.Empty;
    public string GameGenre { get; set; } = string.Empty;
    public string GamePublisher { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
}

public class PurchaseGameResponse
{
    public Guid UserGameId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class PromotionPagedResponse
{
    public IEnumerable<PromotionItemResponse> Items { get; set; } = new List<PromotionItemResponse>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}

public class PromotionItemResponse
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string GameTitle { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsValid { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreatePromotionResponse
{
    public Guid PromotionId { get; set; }
    public string Message { get; set; } = string.Empty;
}
