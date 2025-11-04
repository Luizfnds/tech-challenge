using FCG.Application.Common.Errors;
using FCG.Application.Common.Models;
using FCG.Application.Contracts.Repositories;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Commands.Games.PurchaseGame;

public class PurchaseGameCommandHandler(
    IGameRepository gameRepository,
    IUserRepository userRepository,
    IPromotionRepository promotionRepository) : IRequestHandler<PurchaseGameCommand, Result<Guid>>
{
    private readonly IGameRepository _gameRepository = gameRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPromotionRepository _promotionRepository = promotionRepository;

    public async Task<Result<Guid>> Handle(PurchaseGameCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(ApplicationErrors.User.NotFound(request.UserId));

        var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        if (game is null)
            return Result.Failure<Guid>(ApplicationErrors.Game.NotFound(request.GameId));

        if (!game.IsActive)
            return Result.Failure<Guid>(ApplicationErrors.UserGame.GameNotActive);

        var alreadyOwned = await _gameRepository.UserOwnsGameAsync(request.UserId, request.GameId, cancellationToken);
        if (alreadyOwned)
            return Result.Failure<Guid>(ApplicationErrors.UserGame.AlreadyOwned);

        var finalPrice = game.Price;
        var promotion = await _promotionRepository.GetByGameIdAsync(request.GameId, cancellationToken);
        
        if (promotion != null && promotion.IsValid())
        {
            finalPrice = promotion.CalculateDiscountedPrice(game.Price);
        }

        var userGame = UserGame.Create(request.UserId, request.GameId, finalPrice);

        await _gameRepository.AddUserGameAsync(userGame, cancellationToken);

        var saved = await _gameRepository.SaveChangesAsync(cancellationToken);
        if (!saved)
            return Result.Failure<Guid>(ApplicationErrors.UserGame.PurchaseFailed);

        return Result.Success(userGame.Id);
    }
}
