using Academy.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Academy.Web.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendContactEmailAsync(ContactMessage message)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPortStr = _configuration["EmailSettings:SmtpPort"];
        var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
        var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
        var toEmail = _configuration["EmailSettings:ToEmail"];
        var useSslStr = _configuration["EmailSettings:SmtpUseSsl"];

        // Validate configuration
        if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPortStr) || 
            string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword) || 
            string.IsNullOrEmpty(toEmail) || string.IsNullOrEmpty(useSslStr))
        {
            throw new InvalidOperationException("Email configuration is incomplete. Please check EmailSettings in appsettings.json");
        }

        if (!int.TryParse(smtpPortStr, out var smtpPort))
        {
            throw new InvalidOperationException($"Invalid SMTP port: {smtpPortStr}");
        }

        if (!bool.TryParse(useSslStr, out var useSsl))
        {
            throw new InvalidOperationException($"Invalid SSL setting: {useSslStr}");
        }

        var client = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = useSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var subject = string.IsNullOrEmpty(message.Subject) ? "Contact Form Submission" : message.Subject;
        var body = $"Име: {message.Name}\nИмейл: {message.Email}\nСъобщение: {message.Message}";
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUsername),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        
        mailMessage.To.Add(new MailAddress(toEmail));
        mailMessage.ReplyToList.Add(new MailAddress(message.Email));

        _logger.LogInformation("Преди изпращане на имейл...");
        try
        {
            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Изпратен имейл успешно!");
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP грешка при изпращане на имейл: {Message}", smtpEx.Message);
            throw new InvalidOperationException($"Грешка при изпращане на имейл: {smtpEx.Message}", smtpEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Грешка при изпращане на имейл: {Message}", ex.Message);
            throw new InvalidOperationException($"Грешка при изпращане на имейл: {ex.Message}", ex);
        }
        finally
        {
            client?.Dispose();
            mailMessage?.Dispose();
        }
    }
} 