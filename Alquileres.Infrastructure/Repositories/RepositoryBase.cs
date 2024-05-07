using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Domain.Common;
using Alquileres.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Alquileres.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;

    public RepositoryBase(IDbContextFactory<AppDbContext> factory)
    {
        _context = factory.CreateDbContext();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (predicate != null) return query.AsNoTracking().Where(predicate);

        return query.AsNoTracking();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   string includeString = null,
                                   bool disableTracking = true)
    {
        IQueryable<T> query = _context.Set<T>();

        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null) return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                 Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                 List<Expression<Func<T, object>>> includes = null,
                                 bool disableTracking = true)
    {

        IQueryable<T> query = _context.Set<T>();

        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(id, cancellationToken);
    }

    public virtual async Task<List<T>> GetListByIdAsync(List<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<IReadOnlyList<T>> AddRangeAsync(IReadOnlyList<T> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddRangeAsync(entities);
        await _context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void AddEntity(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void UpdateEntity(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void DeleteEntity(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}
