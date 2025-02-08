using ECommerceApi.Application.DTOs.Order;
using ECommerceApi.Domain.Entities;

namespace ECommerceApi.Application.Abstractions.Services;

public interface IOrderService
{
    Task<ListOrder> GetAllOrderAsync(int page, int size);
    Task<SingleOrder> GetByIdOrderAsync(string id);
    Task CreateOrderAsync(CreateOrder createOrder);
    Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id);
}