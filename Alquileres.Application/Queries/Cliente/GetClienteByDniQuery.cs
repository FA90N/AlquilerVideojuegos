using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.Cliente;

public record class GetClienteByDniQuery(string dni) : IQuery<ClienteFormDTO>;

public class GetClienteByDniQueryHandler : IQueryHandler<GetClienteByDniQuery, ClienteFormDTO>
{
    private readonly IRepositoryBase<Domain.Entities.Cliente> _repository;

    private readonly IMapper _mapper;

    public GetClienteByDniQueryHandler(IRepositoryBase<Domain.Entities.Cliente> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ClienteFormDTO> Handle(GetClienteByDniQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable(x => x.Dni.Equals(request.dni)).AsQueryable();

        var result = query.Select(x => new ClienteFormDTO()
        {
            Id = x.Id,
            Dni = x.Dni,
        });

        return result.FirstOrDefault();

    }
}