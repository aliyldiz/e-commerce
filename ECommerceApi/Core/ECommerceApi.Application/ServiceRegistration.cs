using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceRegistration));
        services.AddHttpClient();
    }
}