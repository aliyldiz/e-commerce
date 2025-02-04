using ECommerceApi.Application.Abstractions.Hubs;
using ECommerceApi.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ECommerceApi.SignalR.HubServices;

public class OrderHubService : IOrderHubService
{
    private readonly IHubContext<OrderHub> _hubContext;

    public OrderHubService(IHubContext<OrderHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task OrderAddedMessageAsync(string message)
        => await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
}