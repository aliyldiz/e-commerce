using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Features.Commands.Order.CreateOrder;
using ECommerceApi.Application.Features.Queries.Order.GetAllOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrder([FromQuery]GetAllOrderQueryRequest request)
    {
        GetAllOrderQueryResponse response = await _mediator.Send(request);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderCommandRequest request)
    {
        CreateOrderCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}