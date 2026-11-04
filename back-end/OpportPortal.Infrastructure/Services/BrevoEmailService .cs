using Microsoft.Extensions.Configuration;
using OpportPortal.Domain.Interfaces.Services;
using System.Net.Http.Json;

namespace OpportPortal.Infrastructure.Services;

public class BrevoEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _senderName;
    private readonly string _senderEmail;

    public BrevoEmailService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _senderName = configuration["Brevo:SenderName"] ?? "OpportPortal";
        _senderEmail = configuration["Brevo:SenderEmail"]
            ?? throw new InvalidOperationException("Brevo:SenderEmail não configurado.");
    }

    public async Task SendAsync(string toEmail, string toName, string subject, string htmlContent)
    {
        try
        {
            var payload = new
            {
                sender = new { name = _senderName, email = _senderEmail },
                to = new[] { new { email = toEmail, name = toName } },
                subject,
                htmlContent
            };

            await _httpClient.PostAsJsonAsync("v3/smtp/email", payload);
        }
        catch
        {
        }
    }
}
