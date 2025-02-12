using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
using ECommerceApi.Application.Features.Commands.Role.CreateRole;
using ECommerceApi.Application.Features.Commands.Role.DeleteRole;
using ECommerceApi.Application.Features.Commands.Role.UpdateRole;
using ECommerceApi.Application.Features.Queries.Role.GetRoleById;
using ECommerceApi.Application.Features.Queries.Role.GetRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
public class RolesController : ControllerBase
{
    readonly IMediator _mediator;
    
    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = "Roles")]
    public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest getRolesQueryRequest)
    {
        GetRolesQueryResponse response = await _mediator.Send(getRolesQueryRequest);
        return Ok(response);
    }
    
    [HttpGet("{Id}")]
    [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Role By Id", Menu = "Roles")]
    public async Task<IActionResult> GetRoles([FromRoute] GetRoleByIdQueryRequest getRoleByIdQueryRequest)
    {
        GetRoleByIdQueryResponse response = await _mediator.Send(getRoleByIdQueryRequest);
        return Ok(response);
    }
    
    [HttpPost()]
    [AuthorizeDefinition(ActionType = ActionType.Writing, Definition = "Create Role", Menu = "Roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest createRoleCommandRequest)
    {
        CreateRoleCommandResponse response = await _mediator.Send(createRoleCommandRequest);
        return Ok(response);
    }
    
    [HttpPut("{Id}")]
    [AuthorizeDefinition(ActionType = ActionType.Updating, Definition = "Update Role", Menu = "Roles")]
    public async Task<IActionResult> UpdateRole([FromBody, FromRoute] UpdateRoleCommandRequest updateRoleCommandRequest)
    {
        UpdateRoleCommandResponse response = await _mediator.Send(updateRoleCommandRequest);
        return Ok(response);
    }
    
    [HttpDelete("{Id}")]
    [AuthorizeDefinition(ActionType = ActionType.Deleting, Definition = "Delete Role", Menu = "Roles")]
    public async Task<IActionResult> DeleteRole([FromRoute] DeleteRoleCommandRequest deleteRoleCommandRequest)
    {
        DeleteRoleCommandResponse response = await _mediator.Send(deleteRoleCommandRequest);
        return Ok(response);
    }
}