namespace FCG.API.Contracts.Requests;

public record UpdateGameRequest(
    string Title,
    string Description,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string Publisher
);
