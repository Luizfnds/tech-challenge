using FCG.Domain.Entities;

namespace FCG.Application.Contracts.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email,  CancellationToken cancellationToken = default);
}