using ECommerceApi.Application.Abstractions.Token;
using ECommerceApi.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly IConfiguration _configuration;

    public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _configuration = configuration;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { _configuration["Google:ClientId"]! },
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
        Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        
        bool result = user is not null;
        if (user is null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user is null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name
                };
                var identityResult = await _userManager.CreateAsync(user); // add to AspNetUsers table
                result = identityResult.Succeeded;
            }
        }

        if (result)
            await _userManager.AddLoginAsync(user, info); // add to AspNetUserLogins table
        else
            throw new Exception("Invalid external authendication.");

        Token token = _tokenHandler.CreateAccessToken(5);
        
        return new()
        {
            Token = token
        };
    }
}