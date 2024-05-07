using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using AutoMapper;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.LineaGenero;

public record class GetLineaGeneroByIdQuery(int Id) : IQuery<List<LineaGeneroDTO>>;
internal class GetLineaGeneroByIdQueryHandler : IQueryHandler<GetLineaGeneroByIdQuery, List<LineaGeneroDTO>>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.LineasGenero> _repository;
    private readonly IMapper _mapper;

    public GetLineaGeneroByIdQueryHandler(IRepositoryBase<Alquileres.Domain.Entities.LineasGenero> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<LineaGeneroDTO>> Handle(GetLineaGeneroByIdQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
           .AsQueryable();

        var data = query.Where(x => x.IdVideojuego == request.Id)
                        .Select(x => new LineaGeneroDTO()
                        {
                            Id = x.Id,
                            IdGenero = x.IdGenero,
                            IdVideojuego = x.IdVideojuego,

                        });


        return (data.ToList());
    }
}
