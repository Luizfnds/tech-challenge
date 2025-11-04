using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Commands.Games.DeactivateGame;

public class DeactivateGameCommandHandler(IGameRepository gameRepository) : IRequestHandler<DeactivateGameCommand, Result>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result> Handle(DeactivateGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.Id, cancellationToken);

        if (game is null)
            return Result.Failure(ApplicationErrors.Game.NotFound(request.Id));

        game.Deactivate();

        await _gameRepository.UpdateAsync(game, cancellationToken);

        return Result.Success();
    }
}
