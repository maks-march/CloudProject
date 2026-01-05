using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi;

public static class DataExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<AppContext>(x =>
        {
            x.UseNpgsql("Host=postgres;Port=5432;Database=MainDB;Username=postgres;Password=123456;");
        });
        
        return serviceCollection;
    }
}