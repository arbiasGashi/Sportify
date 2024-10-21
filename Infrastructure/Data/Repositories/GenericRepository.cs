using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.SpecificationInterfaces;
using Infrastructure.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly SportifyDbContext _context;

    public GenericRepository(SportifyDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<IList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
    {
        // Apply the specification to the query
        var query = ApplySpecification(spec);
        return await query.ToListAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
    }
}