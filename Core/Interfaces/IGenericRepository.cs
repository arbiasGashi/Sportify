using Core.Entities;
using Core.Interfaces.SpecificationInterfaces;
using System.Linq.Expressions;

namespace Core.Interfaces;
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);
    Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    // New method to fetch entities using specifications
    Task<IList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
}
