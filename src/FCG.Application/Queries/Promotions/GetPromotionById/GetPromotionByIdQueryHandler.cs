using MediatR;
using FCG.Application.Contracts.Repositories;
using FCG.Application.Common.Models;
using FCG.Application.Common.Errors;
using FCG.Domain.Entities;

namespace FCG.Application.Queries.Promotions.GetPromotionById;

public class GetPromotionByIdQueryHandler(IPromotionRepository promotionRepository) : IRequestHandler<GetPromotionByIdQuery, Result<Promotion>>
{
    private readonly IPromotionRepository _promotionRepository = promotionRepository;

    public async Task<Result<Promotion>> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId, cancellationToken);

        if (promotion is null)
            return Result.Failure<Promotion>(ApplicationErrors.Promotion.NotFound(request.PromotionId));

        return Result.Success(promotion);
    }
}
