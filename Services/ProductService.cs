using MarketplaceApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MarketplaceApi.Services
{
    public interface IProductService
    {
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        Product Create(CreateProductDto createDto);
        Product? Update(int id, UpdateProductDto updateDto);
        bool Delete(int id);
    }

    public class ProductService : IProductService
    {
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop", Price = 1500 },
            new Product { Id = 2, Name = "Smartphone", Description = "Latest model", Price = 800 },
            new Product { Id = 3, Name = "Headphones", Description = "Noise cancelling", Price = 250 }
        };

        public IEnumerable<Product> GetAll() => _products;

        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

        public Product Create(CreateProductDto createDto)
        {
            var product = new Product
            {
                Id = _products.Count > 0 ? _products.Max(p => p.Id) + 1 : 1,
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                CreatedAt = DateTime.UtcNow
            };
            
            _products.Add(product);
            return product;
        }

        public Product? Update(int id, UpdateProductDto updateDto)
        {
            var product = GetById(id);
            if (product == null) return null;

            if (!string.IsNullOrEmpty(updateDto.Name))
                product.Name = updateDto.Name;
            
            if (!string.IsNullOrEmpty(updateDto.Description))
                product.Description = updateDto.Description;
            
            if (updateDto.Price.HasValue)
                product.Price = updateDto.Price.Value;
            
            if (!string.IsNullOrEmpty(updateDto.ImageUrl))
                product.ImageUrl = updateDto.ImageUrl;

            product.UpdatedAt = DateTime.UtcNow;
            return product;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;
            
            return _products.Remove(product);
        }
    }
}
