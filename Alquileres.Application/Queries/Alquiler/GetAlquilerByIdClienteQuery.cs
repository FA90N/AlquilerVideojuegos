using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Microsoft.EntityFrameworkCore;

namespace Alquileres.Application.Queries.Alquiler;

public record class GetAlquilerByIdClienteQuery(int Id) : IQuery<List<AlquilerListDTO>>;
internal class GetAlquilerByIdClienteQueryHandler : IQueryHandler<GetAlquilerByIdClienteQuery, List<AlquilerListDTO>>
{
    private readonly IRepositoryBase<Domain.Entities.Alquiler> _repository;

    public GetAlquilerByIdClienteQueryHandler(IRepositoryBase<Domain.Entities.Alquiler> repository)
    {
        _repository = repository;
    }

    public async Task<List<AlquilerListDTO>> Handle(GetAlquilerByIdClienteQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
           .Include(x => x.ClienteNavigation)
           .Include(x => x.FormaPagoNavigation)
           .Include(x => x.LineasAlquileres)
           .Where(x => x.IdCliente == request.Id)
           .OrderByDescending(x => x.Fecha)
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

        var result = data.Skip(0).Take(10);
        return result.ToList();
    }
}
