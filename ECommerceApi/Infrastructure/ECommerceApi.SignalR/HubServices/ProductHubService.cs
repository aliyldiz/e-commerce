using ECommerceApi.Application.Abstractions.Hubs;
using ECommerceApi.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ECommerceApi.SignalR.HubServices;

public class ProductHubService : IProductHubService
{
    private readonly IHubContext<ProductHub> _productHub;

    public ProductHubService(IHubContext<ProductHub> productHub)
    {
        _productHub = productHub;
    }

    public async Task ProductAddedMessageAsync(string message)
    {
        await _productHub.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
    }
}