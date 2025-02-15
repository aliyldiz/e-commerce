using ECommerceApi.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint;
using ECommerceApi.Application.Features.Queries.AuthorizationEndpoint;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationEndpointsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorizationEndpointsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetRolesToEndpoint(GetRolesToEndpointQueryRequest rolesToEndpointQueryRequest)
    {
        GetRolesToEndpointQueryResponse response = await _mediator.Send(rolesToEndpointQueryRequest);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> AssignRoleEndpoint(AssignRoleEndpointCommandRequest request)
    {
        request.Type = typeof(Program);
        AssignRoleEndpointCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}