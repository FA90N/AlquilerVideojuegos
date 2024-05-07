using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.PrecioVideoJuego;

public record GetPrecioVideoJuegoByIdQuery(int Id) : IQuery<PrecioVideoJuegoFormDTO>;
internal class GetPrecioVideoJuegoByIdHandler : IQueryHandler<GetPrecioVideoJuegoByIdQuery, PrecioVideoJuegoFormDTO>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.PrecioVideoJuego> _repository;
    private readonly IMapper _mapper;

    public GetPrecioVideoJuegoByIdHandler(IRepositoryBase<Domain.Entities.PrecioVideoJuego> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PrecioVideoJuegoFormDTO> Handle(GetPrecioVideoJuegoByIdQuery req, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(req.Id, cancellationToken);

        return _mapper.Map<PrecioVideoJuegoFormDTO>(result);
    }
}