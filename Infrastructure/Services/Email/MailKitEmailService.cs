using MailKit.Net.Smtp;
using MimeKit;
using MartaPol.Domain.Abstractions;

namespace MartaPol.Infrastructure.Services.Email;

public class MailKitEmailService : IEmailService
{
    private readonly ISettingsService _settings;
    public MailKitEmailService(ISettingsService settings) { _settings = settings; }

    public async Task<bool> SendAsync(IEnumerable<string> attachments, string subject, string to, CancellationToken ct = default)
    {
        var cfg = await _settings.GetAllAsync();
        var pwd = await _settings.GetSmtpPasswordAsync();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MartaPol", cfg.SmtpUser));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = "Wyniki skanowania w załącznikach." };
        foreach (var a in attachments)
            builder.Attachments.Add(a);
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        var useSsl = cfg.SmtpEncryption.Equals("TLS", StringComparison.OrdinalIgnoreCase);
        if (useSsl)
            await client.ConnectAsync(cfg.SmtpHost, cfg.SmtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect, ct);
        else
            await client.ConnectAsync(cfg.SmtpHost, cfg.SmtpPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable, ct);

        if (!string.IsNullOrEmpty(cfg.SmtpUser))
            await client.AuthenticateAsync(cfg.SmtpUser, pwd, ct);

        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);
        return true;
    }
}
