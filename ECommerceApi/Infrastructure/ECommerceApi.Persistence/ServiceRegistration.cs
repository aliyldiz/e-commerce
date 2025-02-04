using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.Abstractions.Services.Authentication;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities.Identity;
using ECommerceApi.Persistence.Contexts;
using ECommerceApi.Persistence.Repositories;
using ECommerceApi.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApi.Persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<ECommerceApiDbContext>(options =>
            options.UseNpgsql(Configuration.ConnectionString));
        
        services.AddIdentity<AppUser, AppRole>(opt =>
        {
            opt.Password.RequiredLength = 3;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireLowercase = false;
        }).AddEntityFrameworkStores<ECommerceApiDbContext>();
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IInvoiceFileRepository, InvoiceFileRepository>();
        services.AddScoped<IProductImageFileRepository, ProductImageFileRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IExternalAuthentication, AuthService>();
        services.AddScoped<IInternalAuthentication, AuthService>();
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddScoped<IBasketItemRepository, BasketItemRepository>();
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
    }
}