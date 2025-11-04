namespace FCG.API.Contracts.Responses;

public record PromotionItemResponse(
    Guid Id,
    Guid GameId,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    bool IsValid,
    DateTime CreatedAt
);
