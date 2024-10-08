using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly SportifyDbContext _context;

    public ProductRepository(SportifyDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<ProductBrand>> GetProductBrandsAsync()
    {
        // Custom logic for fetching product brands
        return await _context.ProductBrands.ToListAsync();
    }

    public async Task<IList<ProductType>> GetProductTypesAsync()
    {
        // Custom logic for fetching product types
        return await _context.ProductTypes.ToListAsync();
    }
}
