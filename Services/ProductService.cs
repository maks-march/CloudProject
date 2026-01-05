using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi;

public class ProductService(AppContext context) : IProductService
{

    public async Task<IEnumerable<Product>> GetAll()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<Product> GetById(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Id не может быть меньше 1");
        
        var product = await context.Products.FindAsync(id);
        if (product == null)
            throw new NotFoundException("Продукт не найден!");
        return product;
    }

    public async Task<Product> Create(CreateProductDto createDto)
    {
        var product = new Product
        {
            Name = createDto.Name,
            Description = createDto.Description,
            Price = createDto.Price,
            CreatedAt = DateTime.UtcNow
        };
        
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Patch(int id, UpdateProductDto updateDto)
    {
        var product = await GetById(id);
        product.Name = updateDto.Name ?? product.Name;
        product.Description = updateDto.Description ?? product.Description;
        product.Price = updateDto.Price ?? product.Price;
        product.ImageUrl = updateDto.ImageUrl ?? product.ImageUrl;

        product.UpdatedAt = DateTime.UtcNow;
        
        context.Update(product);
        await context.SaveChangesAsync();
        return product;
    }
    public async Task<Product> Put(int id, CreateProductDto updateDto)
    {
        var product = await GetById(id);
        product.Name = updateDto.Name;
        product.Description = updateDto.Description;
        product.Price = updateDto.Price;

        product.UpdatedAt = DateTime.UtcNow;
        
        context.Update(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task Delete(int id)
    {
        var product = GetById(id);
        context.Remove(product);
        await context.SaveChangesAsync();
    }
}