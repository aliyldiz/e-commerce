using System.Net;
using System.Net.Mail;
using System.Text;
using ECommerceApi.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendEmailAsync(new[] { to }, subject, body, isBodyHtml);
    }

    public async Task SendEmailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
    {
        MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;
        foreach (var to in tos)
            mail.To.Add(to);
        mail.Subject = subject;
        mail.Body = body;
        mail.From = new(_configuration["Mail:Username"], "admin", System.Text.Encoding.UTF8);
        
        SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
        smtp.Port = 587;
        smtp.EnableSsl = true;
        smtp.Host = _configuration["Mail:Host"];
        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetEmailAsync(string to, string userId, string resetToken)
    {
        StringBuilder mail = new();
        mail.AppendLine(
            $"Hello, <br><br><br> If you are sure you want to reset your password. This link will reset your password. <br><strong><a target=\"_blank\" href=\"");
        mail.AppendLine(_configuration["AngularClientUrl"]);
        mail.AppendLine("/update-password/");
        mail.AppendLine(userId);
        mail.AppendLine("/");
        mail.AppendLine(resetToken);
        mail.AppendLine("\">Click here for new password request.</a></strong><br><br><span style=\"font-size: 12px;\">If you have not received this password reset request, don't take this e-mail seriously.</span><br><br>Best regards.<br><br><br>E-Commerce");
        
        await SendEmailAsync(to, "Password Reset Request", mail.ToString());
    }
}