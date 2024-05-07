using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.LineaAlquiler;

public record class GetLineaAlquilerQuery(int IdAlquiler, LoadDataArgs Args = default) : IQuery<List<LineaAlquilerDTO>>;
internal class GetLineaAlquilerQueryHandler : IQueryHandler<GetLineaAlquilerQuery, List<LineaAlquilerDTO>>
{
    private readonly IRepositoryBase<Domain.Entities.LineasAlquiler> _repository;

    public GetLineaAlquilerQueryHandler(IRepositoryBase<Domain.Entities.LineasAlquiler> repository)
    {
        _repository = repository;
    }

    public async Task<List<LineaAlquilerDTO>> Handle(GetLineaAlquilerQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.PrecioPlataformasNavigation)
            .Where(x => x.IdAlquiler == request.IdAlquiler)
            .AsQueryable();
        var data = query.Select(x => new LineaAlquilerDTO
        {
            Id = x.Id,
            IdAlquiler = x.IdAlquiler,
            IdPrecioVideojuego = x.IdPrecioVideojuego,
            Cantidad = x.Cantidad,
            Juego = x.PrecioPlataformasNavigation.VideoJuegoNavigation.Nombre,
            Plataforma = x.PrecioPlataformasNavigation.PlataformaNavigation.Nombre,
            Precio = x.PrecioPlataformasNavigation.Precio,
            Total = (x.Cantidad * x.PrecioPlataformasNavigation.Precio),

        });

        if (request.Args is null)
        {
            return data.ToList();
        }

        if (!string.IsNullOrEmpty(request.Args.Filter))
        {
            data = data.Where(request.Args.Filter);
        }

        if (!string.IsNullOrEmpty(request.Args.OrderBy))
        {
            data = data.OrderBy(request.Args.OrderBy);
        }

        return data.ToList();
    }
}
