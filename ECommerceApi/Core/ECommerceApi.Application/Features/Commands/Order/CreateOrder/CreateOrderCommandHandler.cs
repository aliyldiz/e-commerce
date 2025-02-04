using ECommerceApi.Application.Abstractions.Hubs;
using ECommerceApi.Application.Abstractions.Services;
using MediatR;

namespace ECommerceApi.Application.Features.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    private readonly IOrderService _service;
    private readonly IBasketService _basketService;
    private readonly IOrderHubService _orderHubService;

    public CreateOrderCommandHandler(IOrderService service, IBasketService basketService, IOrderHubService orderHubService)
    {
        _service = service;
        _basketService = basketService;
        _orderHubService = orderHubService;
    }

    public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
    {
        await _service.CreateOrderAsync(new()
        {
            Address = request.Address,
            Description = request.Description,
            BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
        });

        await _orderHubService.OrderAddedMessageAsync("A new order has been created.");
        
        return new();
    }
}