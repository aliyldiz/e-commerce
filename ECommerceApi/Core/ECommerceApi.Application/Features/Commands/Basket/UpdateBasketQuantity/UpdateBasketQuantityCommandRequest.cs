using MediatR;

namespace ECommerceApi.Application.Features.Commands.Basket.UpdateBasketQuantity;

public class UpdateBasketQuantityCommandRequest : IRequest<UpdateBasketQuantityCommandResponse>
{
    public string BasketItemId { get; set; }
    public int Quantity { get; set; }
}