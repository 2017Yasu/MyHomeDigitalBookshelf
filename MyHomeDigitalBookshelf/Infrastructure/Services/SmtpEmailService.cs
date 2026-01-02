using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Services;

[SingletonService]
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
        // TODO: Implement actual email sending logic here.
        return Task.CompletedTask;
    }
}
