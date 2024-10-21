using Core.Enums;
using Core.Interfaces.SpecificationInterfaces;
using System.Linq.Expressions;

namespace Core.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>> Criteria { get; private set; } = _ => true;
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; private set; } = null;
    public OrderBy OrderByDirection { get; private set; } = Core.Enums.OrderBy.Ascending;
    public int Take { get; private set; } = -1;
    public int Skip { get; private set; } = 0;
    public bool IsPagingEnabled { get; private set; } = false;

    // Ensure this method is accessible to derived classes
    protected void ApplyCriteria(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderBy, OrderBy direction)
    {
        OrderBy = orderBy;
        OrderByDirection = direction;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
