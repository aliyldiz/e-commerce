using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.Order;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;

namespace ECommerceApi.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        await _orderRepository.AddAsync(new Order()
        {
            Address = createOrder.Address,
            Id = Guid.Parse(createOrder.BasketId),
            Description = createOrder.Description
        });
        
        await _orderRepository.SaveChangesAsync();
    }
}