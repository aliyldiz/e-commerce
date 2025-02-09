using ECommerceApi.Application.Consts;
using ECommerceApi.Application.CustomAttributes;
using ECommerceApi.Application.Enums;
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
[Authorize(AuthenticationSchemes = AuthorizeDefinitionConstants.Admin)]
public class BasketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Reading,Definition = "Get Basket Items")]
    public async Task<IActionResult> GetBasketItems([FromQuery]GetBasketItemsQueryRequest request)
    {
        List<GetBasketItemsQueryResponse> response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Writing,Definition = "Add Item to Basket")]
    public async Task<IActionResult> AddItemToBasket(AddItemToBasketCommandRequest request)
    {
        AddItemToBasketCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Updating,Definition = "Update Quantity")]
    public async Task<IActionResult> UpdateQuantity(UpdateBasketQuantityCommandRequest request)
    {
        UpdateBasketQuantityCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{BasketItemId}")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Baskets, ActionType = ActionType.Deleting,Definition = "Remove Basket Item")]
    public async Task<IActionResult> RemoveBasketItem([FromRoute] RemoveBasketItemCommandRequest request)
    {
        RemoveBasketItemCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}