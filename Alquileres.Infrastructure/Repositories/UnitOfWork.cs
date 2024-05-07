using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Domain.Common;
using Alquileres.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Alquileres.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositories;

    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _factory;

    public UnitOfWork(IDbContextFactory<AppDbContext> factory)
    {
        _context = factory.CreateDbContext();
        _factory = factory;
    }

    public AppDbContext AppDbContext => _context;

    public async Task<int> Complete()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IRepositoryBase<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryBase<>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _factory);
            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryBase<TEntity>)_repositories[type];
    }
}
