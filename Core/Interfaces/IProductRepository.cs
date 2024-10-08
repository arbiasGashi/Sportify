using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IList<ProductBrand>> GetProductBrandsAsync();
    Task<IList<ProductType>> GetProductTypesAsync();
}
