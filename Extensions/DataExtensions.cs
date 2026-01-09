using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi;

public static class DataExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection, ConfigurationManager configuration)
    {
        serviceCollection.AddDbContext<AppContext>(x =>
        {
            x.UseNpgsql("Host=localhost;Port=5432;Database=MainDB;Username=march;Password=123456;");
        });
        
        return serviceCollection;
    }
}