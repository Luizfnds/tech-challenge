using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Commands.Games.UpdateGame;

public class UpdateGameCommandHandler(IGameRepository gameRepository) : IRequestHandler<UpdateGameCommand, Result>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result> Handle(UpdateGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var game = await _gameRepository.GetByIdAsync(request.Id, cancellationToken);

            if (game is null)
                return Result.Failure(ApplicationErrors.Game.NotFound(request.Id));

            game.Update(
                request.Title,
                request.Description,
                request.Genre,
                request.Price,
                request.ReleaseDate,
                request.Publisher
            );

            await _gameRepository.UpdateAsync(game, cancellationToken);

            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(Error.Validation("UpdateGame.ValidationError", ex.Message));
        }
        catch (Exception)
        {
            return Result.Failure(ApplicationErrors.Game.UpdateFailed);
        }
    }
}
