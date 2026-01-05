namespace MarketplaceApi;

public interface IProductService
{
    Task<Product> GetById(int id);
    Task<IEnumerable<Product>> GetAll();
    Task<Product> Create(CreateProductDto createDto);
    Task<Product> Patch(int id, UpdateProductDto updateDto);
    
    Task<Product> Put(int id, CreateProductDto updateDto);
    Task Delete(int id);
}