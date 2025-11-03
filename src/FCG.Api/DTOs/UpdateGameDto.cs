namespace FCG.API.DTOs;

public record UpdateGameDto(
    string Title,
    string Description,
    string Genre,
    decimal Price,
    DateTime ReleaseDate,
    string Publisher
);
