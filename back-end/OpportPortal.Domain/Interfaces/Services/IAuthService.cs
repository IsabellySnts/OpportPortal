using OpportPortal.Domain.Entities;

namespace OpportPortal.Domain.Interfaces.Services;

public interface IAuthService
{
    string ComputeHash(string password);
    string GenerateToken(User user);
}
