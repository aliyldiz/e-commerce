using ECommerceApi.Application.Abstractions.Services.Authentication;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IAuthService : IInternalAuthentication, IExternalAuthentication
{
    Task PasswordResetAsync(string email);
    Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
}