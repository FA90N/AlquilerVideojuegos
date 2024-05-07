using Alquileres.Domain.Common;
using System.Linq.Expressions;

namespace Alquileres.Application.Interfaces.Infrastructure.Repositories;

public interface IRepositoryBase<T> where T : BaseEntity
{
    IQueryable<T> GetQueryable(Expression<Func<T, bool>>? predicate = null);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   string includeString = null,
                                   bool disableTracking = true);

    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   List<Expression<Func<T, object>>> includes = null,
                                   bool disableTracking = true);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<List<T>> GetListByIdAsync(List<int> ids, CancellationToken cancellationToken = default);

    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> AddRangeAsync(IReadOnlyList<T> entities, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);

    void AddEntity(T entity);

    void UpdateEntity(T entity);

    void DeleteEntity(T entity);
    Task SaveChanges();
}
