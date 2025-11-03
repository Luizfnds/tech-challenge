using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Infrastructure.Data.Context;
using FCG.Application.Common.Models;

namespace FCG.Infrastructure.Data.Repositories;

public class GameRepository(ApplicationDbContext context) : BaseRepository<Game>(context), IGameRepository
{
    public async Task<PagedResult<Game>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Games.OrderByDescending(g => g.CreatedAt);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Game>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<Game>> GetActiveGamesPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Games
            .Where(g => g.IsActive)
            .OrderByDescending(g => g.CreatedAt);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Game>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<IEnumerable<Game>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default)
    {
        return await _context.Games
            .Where(g => g.Genre == genre && g.IsActive)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserGame?> GetByUserAndGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.UserGames
            .Include(ug => ug.Game)
            .Include(ug => ug.User)
            .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId, cancellationToken);
    }

    public async Task<PagedResult<UserGame>> GetUserGamesPagedAsync(
        Guid userId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.UserGames
            .Include(ug => ug.Game)
            .Where(ug => ug.UserId == userId)
            .OrderByDescending(ug => ug.PurchaseDate);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<UserGame>(items, pageNumber, pageSize, totalCount);
    }

    public async Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default)
    {
        return await _context.UserGames
            .AnyAsync(ug => ug.UserId == userId && ug.GameId == gameId, cancellationToken);
    }

    public async Task AddUserGameAsync(UserGame userGame, CancellationToken cancellationToken = default)
    {
        await _context.UserGames.AddAsync(userGame, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}
