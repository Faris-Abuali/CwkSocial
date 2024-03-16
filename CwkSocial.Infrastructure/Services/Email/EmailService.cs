using CwkSocial.Domain.Services;
using CwkSocial.Infrastructure.Options;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CwkSocial.Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailService(IOptions<EmailConfiguration> options)
    {
        _emailConfig = options.Value;
    }


    /// <summary>
    /// A method takes the MimeMessage object and sends it to the SMTP server
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task Send(MimeMessage message)
    {
        using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            // Establish a secure connection to SMTP server and authenticate
            await smtpClient.ConnectAsync(
                _emailConfig.SmtpServer,
                _emailConfig.Port,
                SecureSocketOptions.StartTls);
            smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            await smtpClient.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

            // Send Email
            await smtpClient.SendAsync(message);
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }


    /// <summary>
    /// A method to prepare the MimeMessage object
    /// </summary>
    /// <param name="recipients"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private MimeMessage CreateMimeMessage(
        List<string> recipients,
        string subject,
        string content)
    {
        // Convert the list of strings to a list of MailboxAddress objects
        var to = new List<MailboxAddress>();

        foreach (var recipient in recipients)
        {
            var name = recipient[..recipient.IndexOf('@')]; // Substring from the start of the string to the first occurrence of '@'
            to.Add(new MailboxAddress(name, recipient));
        }

        var mimeMessage = new MimeMessage
        {
            From = { new MailboxAddress("CwkSocial", _emailConfig.From) },
            Subject = subject,
            Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = content
            },
        };

        mimeMessage.To.AddRange(to);

        return mimeMessage;
    }

    /// <summary>
    /// Another overload of the CreateMimeMessage method, but receives the body as a MimeEntity object
    /// </summary>
    /// <param name="recipients"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    private MimeMessage CreateMimeMessage(
               List<string> recipients,
               string subject,
               MimeEntity body)
    {
        // Convert the list of strings to a list of MailboxAddress objects
        var to = new List<MailboxAddress>();

        foreach (var recipient in recipients)
        {
            var name = recipient[..recipient.IndexOf('@')]; // Substring from the start of the string to the first occurrence of '@'
            to.Add(new MailboxAddress(name, recipient));
        }

        var mimeMessage = new MimeMessage
        {
            From = { new MailboxAddress("CwkSocial", _emailConfig.From) },
            Subject = subject,
            Body = body
        };

        mimeMessage.To.AddRange(to);

        return mimeMessage;
    }

    /// <summary>
    /// High-level Easy-to-use method to send an email
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var recipients = new List<string> { to };

        // 1. Prepare the message in the form of a MimeMessage object
        var mimeMessage = CreateMimeMessage(recipients, subject, body);

        // 2. Send the message
        await Send(mimeMessage);
    }


    /// <summary>
    /// Another overload of the SendEmailAsync method, but receives the body as a MimeEntity object
    /// </summary>
    /// <param name="to"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task SendEmailAsync(string to, string subject, MimeEntity body)
    {
        var recipients = new List<string> { to };

        // 1. Prepare the message in the form of a MimeMessage object
        var mimeMessage = CreateMimeMessage(recipients, subject, body);

        // 2. Send the message
        await Send(mimeMessage);
    }


    public async Task SendEmailConfirmationTokenAsync(
        string to, 
        string confirmationLink)
    {
        var subject = "Confirm Email";
        var body = EmailUtils.GenerateConfirmEmailMessageBody(confirmationLink);

        await SendEmailAsync(to, subject, body);
    }
}
