using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}