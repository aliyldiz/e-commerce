using ECommerceApi.Domain;
using ECommerceApi.Domain.Entities;
using ECommerceApi.Domain.Entities.Common;
using ECommerceApi.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = ECommerceApi.Domain.Entities.File;

namespace ECommerceApi.Persistence.Contexts;

public class ECommerceApiDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public ECommerceApiDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<File> Files { get; set; } // TPH uygulamak için bu ve alttaki iki property'i, bunların entity'lerini yazmak yeterlidir (File base class).
                                           // Sadece File tablosu oluşturulur ve alttaki iki class için tablo oluşturulmaz, bu class'lara eklenen prop'lar File tablosunda column olarak eklenir.
                                           // Ayrıca 3'ü için repository oluşturulur.                                                                                                                                              
    public DbSet<ProductImageFile> ProductImageFiles { get; set; }
    public DbSet<InvoiceFile> InvoiceFiles { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketItem> BasketItems { get; set; }
    public DbSet<CompletedOrder> CompletedOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>()
            .HasKey(o => o.Id);
        
        builder.Entity<Order>()
            .HasIndex(o => o.OrderCode)
            .IsUnique();

        builder.Entity<Basket>()
            .HasOne(o => o.Order)
            .WithOne(b => b.Basket)
            .HasForeignKey<Order>(o => o.Id);
        
        builder.Entity<Order>()
            .HasOne(o => o.CompletedOrder)
            .WithOne(c => c.Order)
            .HasForeignKey<CompletedOrder>(c => c.OrderId);
        
        base.OnModelCreating(builder);
    }

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