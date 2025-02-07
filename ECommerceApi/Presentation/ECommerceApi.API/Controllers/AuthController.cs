using ECommerceApi.Application.Features.Commands.AppUser.FacebookLogin;
using ECommerceApi.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceApi.Application.Features.Commands.AppUser.LoginUser;
using ECommerceApi.Application.Features.Commands.AppUser.PasswordReset;
using ECommerceApi.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerceApi.Application.Features.Commands.AppUser.VerifyResetToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommandRequest request)
    {
        LoginUserCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest request)
    {
        RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest request)
    {
        GoogleLoginCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> FacebookLogin(FacebookLoginCommandRequest request)
    {
        FacebookLoginCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("password-reset")]
    public async Task<IActionResult> PasswordReset([FromBody]PasswordResetCommandRequest request)
    {
        PasswordResetCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("verify-reset-token")]
    public async Task<IActionResult> VerifyResetToken([FromBody]VerifyResetTokenCommandRequest request)
    {
        VerifyResetTokenCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}