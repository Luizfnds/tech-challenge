using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;

namespace FCG.Application.Commands.Promotions.DeactivatePromotion;

public class DeactivatePromotionCommandHandler(IPromotionRepository promotionRepository) : IRequestHandler<DeactivatePromotionCommand, Result>
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;

    public async Task<Result> Handle(DeactivatePromotionCommand request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId, cancellationToken);
        
        if (promotion is null)
            return Result.Failure(ApplicationErrors.Promotion.NotFound(request.PromotionId));

        if (!promotion.IsActive)
            return Result.Failure(ApplicationErrors.Promotion.AlreadyInactive);

        promotion.Deactivate();

        await _promotionRepository.UpdateAsync(promotion, cancellationToken);

        return Result.Success();
    }
}
