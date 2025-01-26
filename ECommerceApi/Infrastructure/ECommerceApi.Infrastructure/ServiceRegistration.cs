using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Infrastructure.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
    }

    public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
}