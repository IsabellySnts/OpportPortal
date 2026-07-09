namespace OpportPortal.Domain.Interfaces.Services;

public interface IAuthService
{
    string ComputeHash(string password);
    string GenerateToken(string email, string type);
}
