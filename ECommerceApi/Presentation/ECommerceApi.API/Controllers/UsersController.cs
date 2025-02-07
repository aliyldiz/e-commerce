using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Features.Commands.AppUser.CreateUser;
using ECommerceApi.Application.Features.Commands.AppUser.UpdatePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMailService _mailService;
    
    public UsersController(IMediator mediator, IMailService mailService)
    {
        _mediator = mediator;
        _mailService = mailService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest request)
    {
        CreateUserCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    // [HttpGet]
    // public async Task<IActionResult> MailTest()
    // {
    //     await _mailService.SendEmailAsync("tager_mail", "sample mail", "content");
    //     return Ok();
    // }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
    {
        UpdatePasswordCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}