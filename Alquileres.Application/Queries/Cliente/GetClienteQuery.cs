using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using Radzen;
using System.Linq.Dynamic.Core;


namespace Alquileres.Application.Queries.Cliente;

public record class GetClienteQuery(LoadDataArgs Args = default) : IQuery<(List<ClienteListDTO>, int)>;

internal class GetClienteQueryHandler : IQueryHandler<GetClienteQuery, (List<ClienteListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.Cliente> _repository;

    public GetClienteQueryHandler(IRepositoryBase<Domain.Entities.Cliente> repository)
    {
        _repository = repository;
    }

    public async Task<(List<ClienteListDTO>, int)> Handle(GetClienteQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .AsQueryable();

        var data = query.Select(x => new ClienteListDTO()
        {
            Id = x.Id,
            FechaAlta = x.FechaAlta,
            Code = x.Code,
            Nombre = x.Nombre,
            Apellidos = x.Apellidos,
            Dni = x.Dni,
            FechaNacimiento = x.FechaNacimiento,
            Comentario = x.Comentario,
            Telefono = x.Telefono,
            Email = x.Email,
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