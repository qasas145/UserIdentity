
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var fromEmail = new MailAddress("your email");
        var fromPassword = "your password";
        var message = new MailMessage();
        message.From = fromEmail;
        message.To.Add(email);
        message.Body = $"<html><body>${htmlMessage}</body></html>";
        message.IsBodyHtml = true;
        var smtpClinet = new SmtpClient("smtp.mail.outlook.com"){
            Port = 587,
            Credentials = new NetworkCredential(fromEmail.ToString(), fromPassword),
            EnableSsl = true,
        };
                
        smtpClinet.Send(message);

    }
}