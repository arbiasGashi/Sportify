using Core.Entities;
using Core.Interfaces.SpecificationInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Specifications;

public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
    {
        var query = inputQuery;

        // Apply Criteria (filtering)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // Apply Includes
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply Sorting
        if (spec.OrderBy != null)
        {
            query = spec.OrderByDirection == Core.Enums.OrderBy.Ascending
                ? query.OrderBy(spec.OrderBy)
                : query.OrderByDescending(spec.OrderBy);
        }

        // Apply Paging
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}
