using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Models;
using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ContentType = MimeKit.ContentType;

namespace Alquileres.Application.Services;

public class SmtpMailSenderService : ISmtpMailSenderService
{
    private readonly EmailConfiguration _emailConfig;

    public SmtpMailSenderService(IConfiguration configuration)
    {
        _emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
    }

    public async Task SendEmailAsync(MessageDto message)
    {
        var mailMessage = CreateEmailMessage(message);

        await SendAsync(mailMessage);
    }

    private MimeMessage CreateEmailMessage(MessageDto message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Bcc.AddRange(message.Bcc);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = message.Content };

        if (message.Attachments != null && message.Attachments.Count != 0)
        {
            foreach (var attachment in message.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment.Item1, attachment.Item3, ContentType.Parse(attachment.Item2));
            }
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false);
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
            await client.SendAsync(mailMessage);
        }
        catch
        {
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }

    private async Task<string> GetAccessToken()
    {
        UserCredential credential;

        using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Data", "google_secret.json"), FileMode.Open, FileAccess.Read))
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                new[] { "https://mail.google.com/" },
                "user",
                CancellationToken.None).ConfigureAwait(false);
        }

        return credential.Token.AccessToken;
    }
}