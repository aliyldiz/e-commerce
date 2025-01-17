using System.Reflection.PortableExecutable;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Persistence.Contexts;

public class ECommerceApiDbContext : DbContext
{
    public ECommerceApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public override int SaveChanges()
    {
        var data = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in data)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedDate = DateTime.UtcNow;
            if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedDate = DateTime.UtcNow;
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var data = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in data)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedDate = DateTime.UtcNow;
            if (entry.State == EntityState.Modified)
                entry.Entity.ModifiedDate = DateTime.UtcNow;
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}