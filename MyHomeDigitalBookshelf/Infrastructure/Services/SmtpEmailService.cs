using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using Microsoft.Extensions.Logging; // Added

namespace MyHomeDigitalBookshelf.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(ILogger<SmtpEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string body)
    {
        _logger.LogInformation("Sending email to {ToEmail} with subject {Subject}. Body: {Body}", toEmail, subject, body);
        // In a real application, this would integrate with an actual SMTP client or email service provider.
        return Task.CompletedTask;
    }
}
