using FCG.Application.Common.Models;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Queries.Promotions.GetPromotionById;

public record GetPromotionByIdQuery(Guid PromotionId) : IRequest<Result<Promotion>>;
