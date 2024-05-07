using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.Genero;

public record GetGeneroByIdQuery(int Id) : IQuery<GeneroFormDTO>;

internal class GetVideoGeneroByIdHandler : IQueryHandler<GetGeneroByIdQuery, GeneroFormDTO>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.Genero> _repository;
    private readonly IMapper _mapper;
    public GetVideoGeneroByIdHandler(IRepositoryBase<Alquileres.Domain.Entities.Genero> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<GeneroFormDTO> Handle(GetGeneroByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<GeneroFormDTO>(result);
    }
}

