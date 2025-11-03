using FCG.Application.Common.Errors;
using FCG.Application.Common.Models;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Commands.Games.PurchaseGame;

public class PurchaseGameCommandHandler : IRequestHandler<PurchaseGameCommand, Result<Guid>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;

    public PurchaseGameCommandHandler(
        IGameRepository gameRepository,
        IUserRepository userRepository)
    {
        _gameRepository = gameRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(PurchaseGameCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(DomainErrors.User.NotFound(request.UserId));

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            return Result.Failure<Guid>(DomainErrors.Game.NotFound(request.GameId));

        if (!game.IsActive)
            return Result.Failure<Guid>(DomainErrors.UserGame.GameNotActive);

        var alreadyOwned = await _gameRepository.UserOwnsGameAsync(request.UserId, request.GameId, cancellationToken);
        if (alreadyOwned)
            return Result.Failure<Guid>(DomainErrors.UserGame.AlreadyOwned);

        var userGame = UserGame.Create(request.UserId, request.GameId, game.Price);

        await _gameRepository.AddUserGameAsync(userGame, cancellationToken);

        var saved = await _gameRepository.SaveChangesAsync(cancellationToken);
        if (!saved)
            return Result.Failure<Guid>(DomainErrors.UserGame.PurchaseFailed);

        return Result.Success(userGame.Id);
    }
}
