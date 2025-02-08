using ECommerceApi.Application.DTOs.Order;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IOrderService
{
    Task<ListOrder> GetAllOrderAsync(int page, int size);
    Task<SingleOrder> GetByIdOrderAsync(string id);
    Task CreateOrderAsync(CreateOrder createOrder);
    Task CompleteOrderAsync(string id);
}