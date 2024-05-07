using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.Plataforma;

public record class GetPlataformaQuery(LoadDataArgs Args = default) : IQuery<(List<PlataformaListDTO>, int)>;

internal class GetVideoPlataformaQueryHandler : IQueryHandler<GetPlataformaQuery, (List<PlataformaListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.Plataforma> _repository;

    public GetVideoPlataformaQueryHandler(IRepositoryBase<Domain.Entities.Plataforma> repository)
    {
        _repository = repository;
    }

    public async Task<(List<PlataformaListDTO>, int)> Handle(GetPlataformaQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .AsQueryable();

        var data = query.Select(x => new PlataformaListDTO()
        {
            Id = x.Id,
            Nombre = x.Nombre,
            Version = x.Version,
            Company = x.Company,
            Activado = x.Activado,
        });

        if (request.Args is null)
        {
            return (data.ToList(), query.Count());
        }

        if (!string.IsNullOrEmpty(request.Args.Filter))
        {
            data = data.Where(request.Args.Filter);
        }

        if (!string.IsNullOrEmpty(request.Args.OrderBy))
        {
            data = data.OrderBy(request.Args.OrderBy);
        }

        var skip = request.Args.Skip ?? 0;
        var take = request.Args.Top ?? 15;
        var result = data.Skip(skip).Take(take).ToList();
        return (result, data.Count());
    }
}
