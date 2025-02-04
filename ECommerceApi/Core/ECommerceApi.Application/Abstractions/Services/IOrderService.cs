using ECommerceApi.Application.DTOs.Order;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrder createOrder);
}