using FCG.Application.Common.Models;
using FCG.Domain.Entities;
using MediatR;

namespace FCG.Application.Queries.Promotions.GetAllPromotions;

public record GetAllPromotionsQuery(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<Result<PagedResult<Promotion>>>;
