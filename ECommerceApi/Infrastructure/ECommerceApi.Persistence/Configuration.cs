using Microsoft.Extensions.Configuration;

namespace ECommerceApi.Persistence;

public static class Configuration
{
    public static string ConnectionString
    {
        get
        {
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ECommerceApi.API"));
            configurationManager.AddJsonFile("appsettings.json");
            
            return configurationManager.GetConnectionString("PostgreSQL");
        }
    } 
}