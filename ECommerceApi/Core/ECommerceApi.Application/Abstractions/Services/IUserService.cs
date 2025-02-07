using ECommerceApi.Application.DTOs.User;
using ECommerceApi.Domain.Entities.Identity;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IUserService
{
    Task<CreateUserResponse> CreateAsync(CreateUser model);
    Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
}