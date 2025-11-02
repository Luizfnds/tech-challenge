using FCG.Domain.Entities;

namespace FCG.Application.Contracts.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
}