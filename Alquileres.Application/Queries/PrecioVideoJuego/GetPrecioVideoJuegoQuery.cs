using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.PrecioVideoJuego;

public record GetPrecioVideoJuegoQuery(int IdVideojuego, LoadDataArgs Args = default) : IQuery<List<PrecioVideoJuegoListDTO>>;
internal class GetPrecioVideoJuegoQueryHandler : IQueryHandler<GetPrecioVideoJuegoQuery, List<PrecioVideoJuegoListDTO>>
{
    private readonly IRepositoryBase<Domain.Entities.PrecioVideoJuego> _repository;


    public GetPrecioVideoJuegoQueryHandler(IRepositoryBase<Domain.Entities.PrecioVideoJuego> repository)
    {
        _repository = repository;

    }
    public async Task<List<PrecioVideoJuegoListDTO>> Handle(GetPrecioVideoJuegoQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.PlataformaNavigation)
            .Where(x => x.IdVideoJuego == request.IdVideojuego)
            .AsQueryable();
        var data = query.Select(x => new PrecioVideoJuegoListDTO
        {
            Id = x.Id,
            NombrePlataforma = x.PlataformaNavigation.Nombre,
            Precio = x.Precio,
            Activado = x.Activado,

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
