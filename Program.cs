using MarketplaceApi;
using Microsoft.EntityFrameworkCore;
using AppContext = MarketplaceApi.AppContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDataAccess(configuration);
builder.Services.AddControllers();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppContext>();
    
    for (int i = 0; i < 10; i++)
    {
        try
        {
            Console.WriteLine($"Migration attempt {i + 1}/10");
            
            if (db.Database.CanConnect())
            {
                db.Database.Migrate();
                Console.WriteLine("Migrations applied successfully");
                break;
            }
            else
            {
                Console.WriteLine("Can't connect to database!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Attempt {i + 1} failed: {ex.Message}");
            if (i == 9) throw;
            Thread.Sleep(5000);
        }
    }
}
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "OK", 
    timestamp = DateTime.UtcNow,
    service = "Marketplace API"
}));

app.Run();
