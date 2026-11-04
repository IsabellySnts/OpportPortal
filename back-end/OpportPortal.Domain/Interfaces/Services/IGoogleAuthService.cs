namespace OpportPortal.Domain.Interfaces.Services;

public record GoogleUserInfo(string Email, string Name);

public interface IGoogleAuthService
{
    Task<GoogleUserInfo?> ValidateAsync(string idToken);
}
