using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class CompletedOrderRepository : GenericRepository<CompletedOrder>, ICompletedOrderRepository
{
    public CompletedOrderRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}