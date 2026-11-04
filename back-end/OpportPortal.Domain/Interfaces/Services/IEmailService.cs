namespace OpportPortal.Domain.Interfaces.Services;

public interface IEmailService
{
    Task SendAsync(string toEmail, string toName, string subject, string htmlContent);
}
