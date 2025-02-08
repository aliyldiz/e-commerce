namespace ECommerceApi.Application.Abstractions.Services;

public interface IMailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true);
    Task SendEmailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
    Task SendPasswordResetEmailAsync(string to, string userId, string resetToken);
    Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userName);
}