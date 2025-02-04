using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.Order;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ListOrder> GetAllOrderAsync(int page, int size)
    {
        var query = _orderRepository.dbSet.Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .Include(o => o.Basket)
            .ThenInclude(b => b.BasketItems)
            .ThenInclude(bi => bi.Product);
            
        var data = query.Skip(page * size).Take(size);

        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data.Select(o => new
            {
                CreatedDate = o.CreatedDate,
                OrderCode = o.OrderCode,
                TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                UserName = o.Basket.User.UserName,
            }).ToListAsync()
        };
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        string orderCode = (new Random().NextDouble() * 100000000).ToString();
        orderCode = orderCode.Substring(orderCode.IndexOf(".") + 1, orderCode.Length - orderCode.IndexOf(".") - 1);
        
        await _orderRepository.AddAsync(new Order()
        {
            Address = createOrder.Address,
            Id = Guid.Parse(createOrder.BasketId),
            Description = createOrder.Description,
            OrderCode = orderCode
        });
        
        await _orderRepository.SaveChangesAsync();
    }
}