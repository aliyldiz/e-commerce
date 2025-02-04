using ECommerceApi.Application.Abstractions.Hubs;
using ECommerceApi.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.SignalR;

public static class ServiceRegistration
{
    public static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddTransient<IProductHubService, ProductHubService>();
        services.AddTransient<IOrderHubService, OrderHubService>();
    }

}