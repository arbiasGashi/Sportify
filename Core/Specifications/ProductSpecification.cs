using Core.Entities;
using System.Linq.Expressions;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(
        string? searchTerm = null,
        string? brand = null,
        string? typeName = null,
        int skip = 0,
        int take = 10,
        string sort = "NameAsc"
    )
    {
        Expression<Func<Product, bool>> criteria = p => true;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            criteria = criteria.AndAlso(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
        }

        if (!string.IsNullOrEmpty(brand))
        {
            criteria = criteria.AndAlso(p => p.ProductBrand.Name.ToLower() == brand.ToLower());
        }

        if (!string.IsNullOrEmpty(typeName))
        {
            criteria = criteria.AndAlso(p => p.ProductType.Name.ToLower() == typeName.ToLower());
        }

        ApplyCriteria(criteria);

        // Include related entities
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);

        // Apply dynamic sorting based on the 'sort' parameter
        ApplySorting(sort);

        // Apply paging
        ApplyPaging(skip, take);
    }

    private void ApplySorting(string sort)
    {
        switch (sort.ToLower())
        {
            case "nameasc":
                ApplyOrderBy(x => x.Name, Enums.OrderBy.Ascending);
                break;
            case "namedesc":
                ApplyOrderBy(x => x.Name, Enums.OrderBy.Descending);
                break;
            case "priceasc":
                ApplyOrderBy(x => x.Price, Enums.OrderBy.Ascending);
                break;
            case "pricedesc":
                ApplyOrderBy(x => x.Price, Enums.OrderBy.Descending);
                break;
            default:
                ApplyOrderBy(x => x.Name, Enums.OrderBy.Ascending);
                break;
        }
    }
}
