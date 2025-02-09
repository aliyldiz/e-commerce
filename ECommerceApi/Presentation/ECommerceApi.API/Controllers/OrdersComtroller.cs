using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
using ECommerceApi.Application.Features.Commands.Order.CompleteOrder;
using ECommerceApi.Application.Features.Commands.Order.CreateOrder;
using ECommerceApi.Application.Features.Queries.Order.GetAllOrder;
using ECommerceApi.Application.Features.Queries.Order.GetByIdOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading,Definition = "Get By Id Order")]
    public async Task<IActionResult> GetByIdOrder([FromRoute]GetByIdOrderQueryRequest request)
    {
        GetByIdOrderQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpGet]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading,Definition = "Get All Order")]
    public async Task<IActionResult> GetAllOrder([FromQuery]GetAllOrderQueryRequest request)
    {
        GetAllOrderQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing,Definition = "Create Order")]
    public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
    {
        CreateOrderCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("complete-order/{id}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating,Definition = "Complete Order")]
    public async Task<IActionResult> CompleteOrder([FromRoute]CompleteOrderCommandRequest request)
    {
        CompleteOrderCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}