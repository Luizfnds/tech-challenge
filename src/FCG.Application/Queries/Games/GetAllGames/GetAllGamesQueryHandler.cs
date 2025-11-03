using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Queries.Games.GetAllGames;

public class GetAllGamesQueryHandler(IGameRepository gameRepository) : IRequestHandler<GetAllGamesQuery, Result<PagedResult<Game>>>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<PagedResult<Game>>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var pagedGames = await _gameRepository.GetAllPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        return Result.Success(pagedGames);
    }
}
