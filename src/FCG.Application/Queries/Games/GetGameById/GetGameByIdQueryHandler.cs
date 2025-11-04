using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Queries.Games.GetGameById;

public class GetGameByIdQueryHandler(IGameRepository gameRepository) : IRequestHandler<GetGameByIdQuery, Result<Game>>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<Game>> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.Id, cancellationToken);

        if (game is null)
            return Result.Failure<Game>(ApplicationErrors.Game.NotFound(request.Id));

        return Result.Success(game);
    }
}
