using MediatR;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Queries.Games.GetAllGames;

public record GetAllGamesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<Game>>>;
