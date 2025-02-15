using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class MenuRepository : GenericRepository<Menu>, IMenuRepository
{
    public MenuRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}