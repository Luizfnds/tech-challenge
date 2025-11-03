using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Queries.Games.GetActiveGames;

public class GetActiveGamesQueryHandler(IGameRepository gameRepository) : IRequestHandler<GetActiveGamesQuery, Result<PagedResult<Game>>>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<PagedResult<Game>>> Handle(GetActiveGamesQuery request, CancellationToken cancellationToken)
    {
        var pagedGames = await _gameRepository.GetActiveGamesPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        return Result.Success(pagedGames);
    }
}
