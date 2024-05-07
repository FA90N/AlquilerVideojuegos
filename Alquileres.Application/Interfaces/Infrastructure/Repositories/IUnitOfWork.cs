using Alquileres.Domain.Common;

namespace Alquileres.Application.Interfaces.Infrastructure.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

    Task<int> Complete();
}
