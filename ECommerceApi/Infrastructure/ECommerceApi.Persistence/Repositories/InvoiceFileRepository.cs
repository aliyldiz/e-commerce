using ECommerceApi.Application.Repositories;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Persistence.Contexts;

namespace ECommerceApi.Persistence.Repositories;

public class InvoiceFileRepository : GenericRepository<InvoiceFile>, IInvoiceFileRepository
{
    public InvoiceFileRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}