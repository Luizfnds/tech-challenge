namespace FCG.API.Contracts.Responses;

public record UserGameItemResponse(
    Guid Id,
    Guid GameId,
    string GameTitle,
    string GameDescription,
    string GameGenre,
    string GamePublisher,
    DateTime PurchaseDate,
    decimal PurchasePrice
);
