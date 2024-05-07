using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alquileres.Application.Queries.PrecioVideoJuego;

public record class GetPrecioVideoJuegoByIdPlatformQuery(int IdPlatform, int IdVideoGame) : IQuery<PrecioVideoJuegoFormDTO>;
internal class GetPrecioVideoJuegoByIdPlatformQueryHandler : IQueryHandler<GetPrecioVideoJuegoByIdPlatformQuery, PrecioVideoJuegoFormDTO>
{
    private readonly IRepositoryBase<Domain.Entities.PrecioVideoJuego> _repository;

    private readonly IMapper _mapper;

    public GetPrecioVideoJuegoByIdPlatformQueryHandler(IRepositoryBase<Domain.Entities.PrecioVideoJuego> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PrecioVideoJuegoFormDTO> Handle(GetPrecioVideoJuegoByIdPlatformQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
             .Include(x => x.PlataformaNavigation).AsQueryable();

        var result = query.Select(x => new PrecioVideoJuegoFormDTO()
        {
            Id = x.Id,
            Activado = x.Activado,
            IdPlataforma = x.IdPlataforma,
            IdVideoJuego = x.IdVideoJuego,
            Precio = x.Precio
        }).Where(x => x.IdPlataforma == request.IdPlatform && x.IdVideoJuego == request.IdVideoGame).FirstOrDefault();

        return result;
    }
}
