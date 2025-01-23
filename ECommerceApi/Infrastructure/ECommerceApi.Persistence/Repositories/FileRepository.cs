using ECommerceApi.Application.Repositories;
using ECommerceApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using File = ECommerceApi.Domain.Entities.File;

namespace ECommerceApi.Persistence.Repositories;

public class FileRepository : GenericRepository<File>, IFileRepository
{
    public FileRepository(ECommerceApiDbContext context) : base(context)
    {
    }
}