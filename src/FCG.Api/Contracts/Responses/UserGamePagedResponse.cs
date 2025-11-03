namespace FCG.API.Contracts.Responses;

public record UserGamePagedResponse(
    IEnumerable<UserGameItemResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);
