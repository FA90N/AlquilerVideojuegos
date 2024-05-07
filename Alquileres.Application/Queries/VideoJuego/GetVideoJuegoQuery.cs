using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.VideoJuego;

public record class GetVideoJuegoQuery(LoadDataArgs Args = default) : IQuery<(List<VideoJuegoListDTO>, int)>;
internal class GetVideoJuegoQueryHandler : IQueryHandler<GetVideoJuegoQuery, (List<VideoJuegoListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.VideoJuego> _repository;

    public GetVideoJuegoQueryHandler(IRepositoryBase<Domain.Entities.VideoJuego> repository)
    {
        _repository = repository;
    }

    public async Task<(List<VideoJuegoListDTO>, int)> Handle(GetVideoJuegoQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .AsQueryable();
        var data = query.Select(x => new VideoJuegoListDTO()
        {
            Id = x.Id,
            Nombre = x.Nombre,
            FechaLanzamiento = x.FechaLanzamiento,
            Desarrollador = x.Desarrollador,
            Descripcion = x.Descripcion,
            Pegi = x.Pegi,
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

