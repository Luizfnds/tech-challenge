using TechChallenge.Domain.Entities;

namespace TechChallenge.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<User> GetByIdAsync(Guid id);
    Task AddAsync(User user);
}