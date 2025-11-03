using FCG.Domain.Entities;
using FCG.Application.Common.Models;

namespace FCG.Application.Contracts.Repositories;

public interface IGameRepository : IBaseRepository<Game>
{
    Task<PagedResult<Game>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedResult<Game>> GetActiveGamesPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Game>> GetByGenreAsync(string genre, CancellationToken cancellationToken = default);
    Task<UserGame?> GetByUserAndGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);
    Task<PagedResult<UserGame>> GetUserGamesPagedAsync(Guid userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<bool> UserOwnsGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken = default);
    Task AddUserGameAsync(UserGame userGame, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}
