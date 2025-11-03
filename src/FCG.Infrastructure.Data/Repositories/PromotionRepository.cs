using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Infrastructure.Data.Context;
using FCG.Application.Common.Models;

namespace FCG.Infrastructure.Data.Repositories;

public class PromotionRepository(ApplicationDbContext context) : BaseRepository<Promotion>(context), IPromotionRepository
{
    public async Task<Promotion?> GetByGameIdAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.Promotions
            .Include(p => p.Game)
            .FirstOrDefaultAsync(p => p.GameId == gameId, cancellationToken);
    }

    public async Task<PagedResult<Promotion>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Promotions
            .Include(p => p.Game)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Promotion>(items, pageNumber, pageSize, totalCount);
    }

    public async Task<PagedResult<Promotion>> GetActivePromotionsPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var query = _context.Promotions
            .Include(p => p.Game)
            .Where(p => p.IsActive && p.StartDate <= now && p.EndDate >= now)
            .OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Promotion>(items, pageNumber, pageSize, totalCount);
    }

    public async Task<bool> GameHasActivePromotionAsync(Guid gameId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Promotions
            .AnyAsync(p => p.GameId == gameId 
                        && p.IsActive 
                        && p.StartDate <= now 
                        && p.EndDate >= now, 
                cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
