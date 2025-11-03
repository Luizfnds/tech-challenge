using FCG.Domain.Entities;

namespace FCG.API.Contracts.Responses;

public record GamePagedResponse(
    IEnumerable<Game> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);
