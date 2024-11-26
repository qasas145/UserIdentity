
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

public class EmailSender : IEmailSender
{
    private readonly MailSettings _options;
    public EmailSender(IOptions<MailSettings> _options) {
        this._options = _options.Value;
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        
        using var smtp = new SmtpClient(_options.Host, _options.Port);
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_options.Email, _options.Password);

        var message = new MailMessage(_options.Email, email, subject, htmlMessage) {
            IsBodyHtml = true,
        };
        
        await smtp.SendMailAsync(message);

    }
}