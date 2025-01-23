using ECommerceApi.Application.Services;
using ECommerceApi.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
    }
}