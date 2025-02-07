using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.User;
using ECommerceApi.Application.Exceptions;
using ECommerceApi.Application.Helpers;
using ECommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserResponse> CreateAsync(CreateUser model)
    {
        IdentityResult result = await _userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName
        }, model.Password);

        CreateUserResponse response = new() { Succeded = result.Succeeded };

        if (result.Succeeded)
            response.Message = "User created successfully";
        else
            foreach (IdentityError error in result.Errors)
                response.Message += $"{error.Code} - {error.Description}\n";
        
        return response;
    }

    public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
    {
        if (user is not null)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
            await _userManager.UpdateAsync(user);
        }
        else
            throw new NotFoundUserException();
    }

    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            resetToken = resetToken.UrlDecode();
            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);
            else
                throw new PasswordChangeFailedException();
        }
    }
}