using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Application.Abstractions.Token;
using ECommerceApi.Infrastructure.Services.Storage;
using ECommerceApi.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.Infrastructure;

public static class ServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ITokenHandler, TokenHandler>();
    }

    public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
}