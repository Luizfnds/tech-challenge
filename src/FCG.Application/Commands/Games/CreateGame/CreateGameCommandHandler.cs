using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Commands.Games.CreateGame;

public class CreateGameCommandHandler(IGameRepository gameRepository) : IRequestHandler<CreateGameCommand, Result<Guid>>
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public async Task<Result<Guid>> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var game = Game.Create(
                request.Title,
                request.Description,
                request.Genre,
                request.Price,
                request.ReleaseDate,
                request.Publisher
            );

            await _gameRepository.AddAsync(game, cancellationToken);

            return Result.Success(game.Id);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<Guid>(Error.Validation("CreateGame.ValidationError", ex.Message));
        }
        catch (Exception)
        {
            return Result.Failure<Guid>(DomainErrors.Game.CreationFailed);
        }
    }
}
