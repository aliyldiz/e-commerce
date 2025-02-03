using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
{
    public BasketItemRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}