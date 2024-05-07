using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using AutoMapper;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.Genero;

public record GetGeneroQuery(LoadDataArgs Args = default) : IQuery<(List<GeneroListDTO>, int)>;

internal class GetVideoGeneroQueryHandler : IQueryHandler<GetGeneroQuery, (List<GeneroListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.Genero> _repository;
    private readonly IMapper _mapper;
    public GetVideoGeneroQueryHandler(IRepositoryBase<Domain.Entities.Genero> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<GeneroListDTO>, int)> Handle(GetGeneroQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .AsQueryable();

        if (request.Args is null)
        {
            return (_mapper.Map<List<GeneroListDTO>>(query.ToList()), query.Count());
        }

        if (!string.IsNullOrEmpty(request.Args.Filter))
        {
            query = query.Where(request.Args.Filter);
        }

        if (!string.IsNullOrEmpty(request.Args.OrderBy))
        {
            query = query.OrderBy(request.Args.OrderBy);
        }

        var skip = request.Args.Skip ?? 0;
        var take = request.Args.Top ?? 15;

        var result = query.Skip(skip).Take(take).ToList();

        return (_mapper.Map<List<GeneroListDTO>>(result), query.Count());
    }
}



