using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using OpportPortal.Domain.Interfaces.Services;

namespace OpportPortal.Infrastructure.Auth;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly string _clientId;

    public GoogleAuthService(IConfiguration configuration)
    {
        _clientId = configuration["Google:ClientId"]
            ?? throw new InvalidOperationException("Google:ClientId não configurado.");
    }

    public async Task<GoogleUserInfo?> ValidateAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _clientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return new GoogleUserInfo(payload.Email, payload.Name ?? "");
        }
        catch
        {
            return null;
        }
    }
}
