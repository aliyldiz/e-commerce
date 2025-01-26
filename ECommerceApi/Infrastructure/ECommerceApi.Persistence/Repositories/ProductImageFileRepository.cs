using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class ProductImageFileRepository : GenericRepository<ProductImageFile>, IProductImageFileRepository
{
    public ProductImageFileRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}