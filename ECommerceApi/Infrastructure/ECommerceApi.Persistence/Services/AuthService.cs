using System.Text.Json;
using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Abstractions.Token;
using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.DTOs.Facebook;
using ECommerceApi.Application.Exceptions;
using ECommerceApi.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Persistence.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler; 
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;

    public AuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _userService = userService;
    }

    public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
    {
        AppUser? user = await _userManager.FindByNameAsync(userNameOrEmail);
        if (user is null)
            user = await _userManager.FindByEmailAsync(userNameOrEmail);

        if (user is null)
            throw new NotFoundUserException(); 
        
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        if (result.Succeeded)
        {
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 10);
            return token;
        }
        
        throw new AuthenticationErrorException();
    }

    public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user is not null && user?.RefreshTokenEndDate > DateTime.UtcNow)
        {
            Token token = _tokenHandler.CreateAccessToken(15, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
            return token;
        }
        throw new NotFoundUserException();
    }

    public async Task<Token> GoogleLoginAsync(string idToken, string provider, int accessTokenLifeTime)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { _configuration["ExternalLoginSettings:Google:ClientId"]! },
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

        var info = new UserLoginInfo(provider, payload.Subject, provider);
        AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        
        return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
    }

    public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
    {
        string accessTokenResponse = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:ClientId"]}" +
            $"&client_secret={_configuration["ExternalLoginSettings:Facebook:ClientSecret"]}" +
            $"&grant_type=client_credentials");
        
        FacebookAccessTokenResponse? facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

        string userAccessTokenValidation = await _httpClient.GetStringAsync(
            $"https://graph.facebook.com/debug_token?input_token={authToken}" +
            $"&access_token={facebookAccessToken?.AccessToken}");
        
        FacebookUserAccessTokenValidation? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessTokenValidation);
        
        if (validation?.Data.IsValid != null) // IsValid will enter the block according to true false, 
        {
            string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
            FacebookUserInfoResponse? userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);
            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifeTime);
        }
        
        throw new Exception("Invalid external authentication.");
    }

    async Task<Token> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
    {
        bool result = user != null;
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = email,
                    UserName = email,
                    FullName = name
                };
                var identityResult = await _userManager.CreateAsync(user); // add to AspNetUsers table
                result = identityResult.Succeeded;
            }
        }
        if (result)
        {
            await _userManager.AddLoginAsync(user, info); // add to AspNetUserLogins table
            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 10);
            return token;
        }
        
        throw new Exception("Invalid external authentication.");
    }
}