using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.Alquiler;

public record GetAlquilerByIdQuery(int Id) : IQuery<AlquilerFormDTO>;
internal class GetAlquilerByIdQueryHandler : IQueryHandler<GetAlquilerByIdQuery, AlquilerFormDTO>
{

    private readonly IRepositoryBase<Domain.Entities.Alquiler> _repository;
    private readonly IMapper _mapper;

    public GetAlquilerByIdQueryHandler(IRepositoryBase<Domain.Entities.Alquiler> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<AlquilerFormDTO> Handle(GetAlquilerByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<AlquilerFormDTO>(result);
    }
}
