using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.LineaAlquiler;

public record GetLineaAlquilerByIdQuery(int Id) : IQuery<LineaAlquilerFormDTO>;
internal class GetLineaAlquilerByIdQueryHandler : IQueryHandler<GetLineaAlquilerByIdQuery, LineaAlquilerFormDTO>
{
    private readonly IRepositoryBase<Domain.Entities.LineasAlquiler> _repository;
    private readonly AutoMapper.IMapper _mapper;

    public GetLineaAlquilerByIdQueryHandler(IRepositoryBase<Domain.Entities.LineasAlquiler> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<LineaAlquilerFormDTO> Handle(GetLineaAlquilerByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<LineaAlquilerFormDTO>(result);
    }
}
