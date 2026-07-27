using Microsoft.EntityFrameworkCore;
using OpportPortal.Domain.Entities;
using OpportPortal.Domain.Interfaces.Repositories;

namespace OpportPortal.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly OpportPortalDbContext _context;

    public UserRepository(OpportPortalDbContext context)
    {
        _context = context;
    }

    public async Task<long> Create(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await _context.Users
            .SingleOrDefaultAsync(u => u.UserEmail.Value == email);

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _context.Users
            .AnyAsync(u => u.UserEmail.Value == email);

    public async Task<User?> GetByIdAsync(long id)
        => await _context.Users.FindAsync(id);
}
