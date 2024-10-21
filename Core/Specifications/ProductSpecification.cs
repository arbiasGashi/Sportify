using Core.Entities;
using Core.Enums;

namespace Core.Specifications;
public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string searchTerm, string brand, string typeName, int skip, int take, OrderBy orderBy)
         : base(x =>
             (string.IsNullOrEmpty(searchTerm) || x.Name.ToLower().Contains(searchTerm.ToLower())) &&
             (string.IsNullOrEmpty(brand) || x.ProductBrand.Name.ToLower() == brand.ToLower()) &&
             (string.IsNullOrEmpty(typeName) || x.ProductType.Name.ToLower() == typeName.ToLower())
         )
    {
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);

        ApplyPaging(skip, take);

        if (orderBy == Core.Enums.OrderBy.Ascending)
        {
            AddOrderBy(x => x.Name);
        }
        else
        {
            AddOrderByDescending(x => x.Name);
        }
    }
}
