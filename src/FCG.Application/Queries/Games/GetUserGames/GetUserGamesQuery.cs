using FCG.Application.Common.Models;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Queries.Games.GetUserGames;

public record GetUserGamesQuery(
    Guid UserId, 
    int PageNumber = 1, 
    int PageSize = 10) : IRequest<Result<PagedResult<UserGame>>>;
