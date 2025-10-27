using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Repositories;
using TechChallenge.Infrastructure.Data.Context;

namespace TechChallenge.Infrastructure.Data.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }
}
