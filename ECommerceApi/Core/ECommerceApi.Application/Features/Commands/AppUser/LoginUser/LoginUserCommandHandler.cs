using ECommerceApi.Application.Abstractions.Token;
using ECommerceApi.Application.DTOs;
using ECommerceApi.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser?> _userManager;
    private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler; 

    public LoginUserCommandHandler(
        UserManager<Domain.Entities.Identity.AppUser?> userManager, 
        SignInManager<Domain.Entities.Identity.AppUser> signInManager, 
        ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Identity.AppUser? user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
        if (user is null)
            user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);

        if (user is null)
            throw new NotFoundUserException(); 
        
        SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
        {
            Token token = _tokenHandler.CreateAccessToken(5);
            
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }

        // return new LoginUserErrorCommandResponse()
        // {
        //     Message = "Login failed"
        // };
        
        throw new AuthenticationErrorException();
    }
}