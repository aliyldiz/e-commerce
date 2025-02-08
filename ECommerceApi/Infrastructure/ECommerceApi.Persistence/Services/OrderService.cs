using ECommerceApi.Application.Abstractions.Services;
using ECommerceApi.Application.DTOs.Order;
using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICompletedOrderRepository _completedOrderRepository;

    public OrderService(IOrderRepository orderRepository, ICompletedOrderRepository completedOrderRepository)
    {
        _orderRepository = orderRepository;
        _completedOrderRepository = completedOrderRepository;
    }

    public async Task<ListOrder> GetAllOrderAsync(int page, int size)
    {
        var query = _orderRepository.dbSet.Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .Include(o => o.Basket)
            .ThenInclude(b => b.BasketItems)
            .ThenInclude(bi => bi.Product);
            
        var data = query.Skip(page * size).Take(size);

        var data2 = from order in data
            join completedOrder in _completedOrderRepository.dbSet
                on order.Id equals completedOrder.OrderId into co
            from _co in co.DefaultIfEmpty()
            select new
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                OrderCode = order.OrderCode,
                Basket = order.Basket,
                Completed = _co != null ? true : false
            };
        
        return new()
        {
            TotalOrderCount = await query.CountAsync(),
            Orders = await data2.Select(o => new
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                OrderCode = o.OrderCode,
                TotalPrice = o.Basket.BasketItems.Sum(bi => bi.Product.Price * bi.Quantity),
                UserName = o.Basket.User.UserName,
                o.Completed
            }).ToListAsync()
        };
    }

    public async Task<SingleOrder> GetByIdOrderAsync(string id)
    {
        var data = _orderRepository.dbSet
            .Include(o => o.Basket)
            .ThenInclude(b => b.BasketItems)
            .ThenInclude(bi => bi.Product);
       
        var data2 = await (from order in data
            join completedOrder in _completedOrderRepository.dbSet
                on order.Id equals completedOrder.OrderId into co
            from _co in co.DefaultIfEmpty()
            select new
            {
                Id = order.Id,
                CreatedDate = order.CreatedDate,
                OrderCode = order.OrderCode,
                Basket = order.Basket,
                Completed = _co != null ? true : false,
                Address = order.Address,
                Description = order.Description
            }).FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));

        return new()
        {
            Id = data2.Id.ToString(),
            BasketItems = data2.Basket.BasketItems.Select(bi => new
            {
                bi.Product.Name,
                bi.Product.Price,
                bi.Quantity,
            }),
            Address = data2.Address,
            CreatedDate = data2.CreatedDate,
            Description = data2.Description,
            OrderCode = data2.OrderCode, 
            Completed = data2.Completed,
        };
    }

    public async Task CreateOrderAsync(CreateOrder createOrder)
    {
        string orderCode = (new Random()).Next(10000, 99999).ToString();
        
        await _orderRepository.AddAsync(new Order()
        {
            Address = createOrder.Address,
            Id = Guid.Parse(createOrder.BasketId),
            Description = createOrder.Description,
            OrderCode = orderCode
        });
        
        await _orderRepository.SaveChangesAsync();
    }

    public async Task<(bool, CompletedOrderDTO)> CompleteOrderAsync(string id)
    {
        Order? order = await _orderRepository.dbSet
            .Include(o => o.Basket)
            .ThenInclude(b => b.User)
            .FirstOrDefaultAsync(o => o.Id == Guid.Parse(id));
        
        if (order is not null)
        {
            await _completedOrderRepository.AddAsync(new CompletedOrder() { OrderId = Guid.Parse(id) });
            await _completedOrderRepository.SaveChangesAsync();
            return (true, new()
            {
                OrderCode = order.OrderCode,
                OrderDate = order.CreatedDate,
                Username = order.Basket.User.UserName,
                EMail = order.Basket.User.Email,
            });
        }
        return (false, null);
    }
}