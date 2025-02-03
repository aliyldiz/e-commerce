using ECommerceApi.Application.Features.Commands.Basket.AddItemToBasket;
using ECommerceApi.Application.Features.Commands.Basket.RemoveBasketItem;
using ECommerceApi.Application.Features.Commands.Basket.UpdateBasketQuantity;
using ECommerceApi.Application.Features.Queries.Basket.GetBasketItems;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")]
public class BasketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBasketItems([FromQuery]GetBasketItemsQueryRequest request)
    {
        List<GetBasketItemsQueryResponse> response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
    {
        AddItemToBasketCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateQuantity(UpdateBasketQuantityCommandRequest request)
    {
        UpdateBasketQuantityCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{BasketItemId}")]
    public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest request)
    {
        RemoveBasketItemCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}