using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class EndpointRepository : GenericRepository<Endpoint>, IEndpointRepository
{
    public EndpointRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}