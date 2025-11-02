using Microsoft.EntityFrameworkCore;
using FCG.Domain.Entities;
using FCG.Application.Contracts.Repositories;
using FCG.Infrastructure.Data.Context;

namespace FCG.Infrastructure.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await base.GetByIdAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await base.AddAsync(user);
    }
}
