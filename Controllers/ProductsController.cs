using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productService.GetAll();
            return Ok(products);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await productService.GetById(id);
            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await productService.Create(createDto);
            return Ok(product);
        }

        // PATCH: api/products/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] UpdateProductDto updateDto)
        {
            var product = await productService.Patch(id, updateDto);
            return Ok(product);
        }

        // PUT: api/products/5 (альтернатива PATCH)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CreateProductDto createDto)
        {
            try
            {
                var product = await productService.GetById(id);
                product = await productService.Put(id, createDto);
                return Ok(product);
            }
            catch
            {
                var product = await productService.Create(createDto);
                return Ok(product);
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await productService.Delete(id);
            return NoContent();
        }
    }
}
