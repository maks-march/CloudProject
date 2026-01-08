using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi;

public static class DataExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, ConfigurationManager configuration)
    {
        serviceCollection.AddDbContext<AppContext>(x =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            x.UseNpgsql(connectionString);
        });
        
        return serviceCollection;
    }
}