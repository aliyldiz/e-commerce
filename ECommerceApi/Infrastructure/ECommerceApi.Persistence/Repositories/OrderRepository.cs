using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}