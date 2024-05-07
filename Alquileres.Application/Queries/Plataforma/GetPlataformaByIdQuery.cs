using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.Plataforma;

public record GetPlataformaByIdQuery(int Id) : IQuery<PlataformaFormDTO>;
internal class GetVideoPlataformaByIdQueryHandler : IQueryHandler<GetPlataformaByIdQuery, PlataformaFormDTO>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.Plataforma> _repository;
    private readonly IMapper _mapper;
    public GetVideoPlataformaByIdQueryHandler(IRepositoryBase<Domain.Entities.Plataforma> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PlataformaFormDTO> Handle(GetPlataformaByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<PlataformaFormDTO>(result);
    }
}

