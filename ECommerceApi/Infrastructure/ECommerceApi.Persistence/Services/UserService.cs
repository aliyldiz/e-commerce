using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.User;
using ECommerceApi.Application.Exceptions;
using ECommerceApi.Application.Helpers;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEndpointRepository _endpointRepository;

    public UserService(UserManager<AppUser> userManager, IEndpointRepository endpointRepository)
    {
        _userManager = userManager;
        _endpointRepository = endpointRepository;
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

    public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
    {
        var users = await _userManager.Users
            .Skip(page * size)
            .Take(size)
            .ToListAsync();

        return users.Select(user => new ListUser
        {
            Id = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            TwoFactorEnabled = user.TwoFactorEnabled,
            UserName = user.UserName,
        }).ToList();
    }

    public int TotalUsersCount => _userManager.Users.Count();
    
    public async Task AssignRoleToUserAsync(string userId, string[] roles)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user is not null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            
            await _userManager.AddToRolesAsync(user, roles);
        }
    }

    public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
    {
        AppUser user = await _userManager.FindByIdAsync(userIdOrName);
        if (user is null)
            user = await _userManager.FindByNameAsync(userIdOrName);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }
        return new string[] { };
    }

    public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
    {
        var userRoles = await GetRolesToUserAsync(name);
        
        if (!userRoles.Any())
            return false;
        
        Endpoint? endpoint = await _endpointRepository.dbSet.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code);

        if (endpoint == null)
            return false;

        var hasRole = false;
        var endpointRoles = endpoint.Roles.Select(r => r.Name);

        foreach (var userRole in userRoles)
        {
            foreach (var endpointRole in endpointRoles)
                if (userRole == endpointRole)
                    return true;
        }
        
        return false;
    }
}