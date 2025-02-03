using ECommerceApi.Application.Abstractions.Services;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Basket.UpdateBasketQuantity;

public class UpdateBasketQuantityCommandHandler : IRequestHandler<UpdateBasketQuantityCommandRequest, UpdateBasketQuantityCommandResponse>
{
    private readonly IBasketService _basketService;

    public UpdateBasketQuantityCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<UpdateBasketQuantityCommandResponse> Handle(UpdateBasketQuantityCommandRequest request, CancellationToken cancellationToken)
    {
        await _basketService.UpdateBasketQuantityAsync(new()
        {
            BasketItemId = request.BasketItemId,
            Quantity = request.Quantity
        });
        
        return new();
    }
}