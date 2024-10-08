using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Sportify.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductsController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: api/v1/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await _productRepository.GetAllAsync(p => p.ProductType, p => p.ProductBrand);
        return Ok(products);
    }

    // GET: api/v1/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await _productRepository.GetByIdAsync(id, p => p.ProductType, p => p.ProductBrand);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // POST: api/v1/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product newProduct)
    {
        await _productRepository.AddAsync(newProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
    }

    // PUT: api/v1/products/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        // Update product properties
        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.Price = updatedProduct.Price;
        product.PictureUrl = updatedProduct.PictureUrl;
        product.ProductTypeId = updatedProduct.ProductTypeId;
        product.ProductBrandId = updatedProduct.ProductBrandId;

        await _productRepository.UpdateAsync(product);

        return NoContent();
    }

    // DELETE: api/v1/products/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        await _productRepository.DeleteAsync(product);

        return NoContent();
    }
}
