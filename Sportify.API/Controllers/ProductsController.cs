using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Sportify.API.DTOs;

namespace Sportify.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    // GET: api/v1/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts(
        string? searchTerm = null,
        string? brand = null,
        string? type = null,
        int skip = 0,
        int take = 10,
        string sort = "NameAsc")
    {
        // Create the updated specification with the new sort parameter
        var spec = new ProductSpecification(searchTerm, brand, type, skip, take, sort);

        // Fetch products using specification
        var products = await _productRepository.GetAllWithSpecAsync(spec);

        // Map products to ProductDTO
        var productsToReturn = _mapper.Map<IList<ProductDTO>>(products);

        return Ok(productsToReturn);
    }

    // GET: api/v1/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById(int id)
    {
        // Include related entities directly in the repository call
        var product = await _productRepository.GetByIdAsync(id, p => p.ProductType, p => p.ProductBrand);

        if (product == null)
        {
            return NotFound();
        }

        // Map to ProductDTO
        var productToReturn = _mapper.Map<ProductDTO>(product);

        return Ok(productToReturn);
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
