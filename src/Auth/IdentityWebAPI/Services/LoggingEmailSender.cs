namespace IdentityWebAPI.Services;

public class LoggingEmailSender : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger;

    public LoggingEmailSender(ILogger<LoggingEmailSender> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string body)
    {
        _logger.LogInformation(
            "Email to {ToEmail} | Subject: {Subject} | Body: {Body}",
            toEmail,
            subject,
            body);

        return Task.CompletedTask;
    }
}
