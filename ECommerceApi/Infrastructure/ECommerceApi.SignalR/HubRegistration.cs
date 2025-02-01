using ECommerceApi.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace ECommerceApi.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication application)
    {
        application.MapHub<ProductHub>("/products-hub");
    }
}