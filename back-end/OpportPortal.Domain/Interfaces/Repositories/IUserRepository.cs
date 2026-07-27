using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<long> Create(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByIdAsync(long id);
}
