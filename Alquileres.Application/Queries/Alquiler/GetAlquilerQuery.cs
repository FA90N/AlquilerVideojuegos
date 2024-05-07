using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.Alquiler;

public record class GetAlquilerQuery(LoadDataArgs Args = default) : IQuery<(List<AlquilerListDTO>, int)>;
internal class GetAlquilerQueryHandler : IQueryHandler<GetAlquilerQuery, (List<AlquilerListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.Alquiler> _repository;

    public GetAlquilerQueryHandler(IRepositoryBase<Domain.Entities.Alquiler> repository)
    {
        _repository = repository;
    }

    public async Task<(List<AlquilerListDTO>, int)> Handle(GetAlquilerQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .Include(x => x.ClienteNavigation)
            .Include(x => x.FormaPagoNavigation)
            .Include(x => x.LineasAlquileres)
            .AsQueryable();

        var data = query.Select(x => new AlquilerListDTO()
        {
            Id = x.Id,
            Codigo = x.Code,
            Fecha = x.Fecha,
            NombreCliente = x.ClienteNavigation.Nombre + " " + x.ClienteNavigation.Apellidos,
            DNI = x.ClienteNavigation.Dni,
            FormaPago = x.FormaPagoNavigation.Nombre,
            Total = (decimal)(x.LineasAlquileres.Sum(x => x.Cantidad * x.PrecioPlataformasNavigation.Precio) * x.Dias),
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