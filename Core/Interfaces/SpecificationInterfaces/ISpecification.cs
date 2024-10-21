using Core.Enums;
using System.Linq.Expressions;

namespace Core.Interfaces.SpecificationInterfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    OrderBy OrderByDirection { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
