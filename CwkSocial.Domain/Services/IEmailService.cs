namespace CwkSocial.Domain.Services;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);

    Task SendEmailConfirmationTokenAsync(string to, string url);
}
