using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Contracts.Repositories;

public interface IPromotionRepository : IBaseRepository<Promotion>
{
    Task<Promotion?> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<PagedResult<Promotion>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Promotion>> GetActivePromotionsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> GameHasActivePromotionAsync(Guid gameId, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
