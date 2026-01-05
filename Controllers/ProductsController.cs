using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            
            var product = _productService.GetById(id);
            return product != null ? Ok(product) : NotFound();
        }

        // POST: api/products
        [HttpPost]
        public IActionResult Create([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _productService.Create(createDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PATCH: api/products/5
        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateProductDto updateDto)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            
            var product = _productService.Update(id, updateDto);
            return product != null ? Ok(product) : NotFound();
        }

        // PUT: api/products/5 (альтернатива PATCH)
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateProductDto createDto)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            
            // Для PUT - сначала удаляем, потом создаем новый
            _productService.Delete(id);
            var product = _productService.Create(createDto);
            product.Id = id;
            
            return Ok(product);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            
            var deleted = _productService.Delete(id);
            return deleted ? NoContent() : NotFound();
        }

        // Health check для балансировщика
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new 
            { 
                status = "OK", 
                timestamp = DateTime.UtcNow,
                service = "Products API",
                version = "1.0"
            });
        }
    }
}
