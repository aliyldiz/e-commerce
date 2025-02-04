using ECommerceApi.Application.DTOs.Order;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IOrderService
{
    Task<ListOrder> GetAllOrderAsync(int page, int size);
    Task CreateOrderAsync(CreateOrder createOrder);
}