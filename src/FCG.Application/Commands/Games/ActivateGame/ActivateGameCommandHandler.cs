using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Commands.Games.ActivateGame;

public class ActivateGameCommandHandler(IGameRepository gameRepository) : IRequestHandler<ActivateGameCommand, Result>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result> Handle(ActivateGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetByIdAsync(request.Id, cancellationToken);

        if (game is null)
            return Result.Failure(DomainErrors.Game.NotFound(request.Id));

        game.Activate();

        await _gameRepository.UpdateAsync(game, cancellationToken);

        return Result.Success();
    }
}
