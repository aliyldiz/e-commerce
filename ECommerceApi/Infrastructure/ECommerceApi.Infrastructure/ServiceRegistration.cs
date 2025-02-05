using System.Net.Mail;
using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Abstractions.Storage;
using ECommerceApi.Application.Abstractions.Token;
using ECommerceApi.Infrastructure.Services;
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
        services.AddScoped<IMailService, MailService>();
    }

    public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
    {
        services.AddScoped<IStorage, T>();
    }
}