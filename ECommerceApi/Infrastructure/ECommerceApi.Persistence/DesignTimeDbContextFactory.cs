using ECommerceApi.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace ECommerceApi.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ECommerceApiDbContext>
{
    public ECommerceApiDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ECommerceApiDbContext> builder = new();
        builder.UseNpgsql(Configuration.ConnectionString);
        return new(builder.Options);
    }
}
