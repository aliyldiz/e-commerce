using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class BasketRepository : GenericRepository<Basket>, IBasketRepository
{
    public BasketRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}