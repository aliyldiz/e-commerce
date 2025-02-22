using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
using ECommerceApi.Application.Features.Commands.AppUser.AssignRoleToUser;
using ECommerceApi.Application.Features.Commands.AppUser.CreateUser;
using ECommerceApi.Application.Features.Commands.AppUser.UpdatePassword;
using ECommerceApi.Application.Features.Queries.AppUser.GetAllUsers;
using ECommerceApi.Application.Features.Queries.AppUser.GetRolesToUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get All Users")]
    public async Task<IActionResult> GetAllUsers([FromQuery]GetAllUsersQueryRequest request)
    {
        GetAllUsersQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpGet("get-roles-to-user/{UserId}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Get Roles To Users")]
    public async Task<IActionResult> GetRolesToUser([FromRoute]GetRolesToUserQueryRequest getRolesToUserQueryRequest)
    {
        GetRolesToUserQueryResponse response = await _mediator.Send(getRolesToUserQueryRequest);
        return Ok(response);
    }
    
    [HttpPost("assign-role-to-user")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Users, ActionType = ActionType.Reading, Definition = "Assign Role To User")]
    public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest request)
    {
        AssignRoleToUserCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}