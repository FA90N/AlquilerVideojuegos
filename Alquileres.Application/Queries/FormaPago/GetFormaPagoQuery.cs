using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using AutoMapper;
using Radzen;
using System.Linq.Dynamic.Core;

namespace Alquileres.Application.Queries.FormaPago;

public record GetFormaPagoQuery(LoadDataArgs Args = default) : IQuery<(List<FormaPagoListDTO>, int)>;

internal class GetFormaPagoQueryHandler : IQueryHandler<GetFormaPagoQuery, (List<FormaPagoListDTO>, int)>
{
    private readonly IRepositoryBase<Domain.Entities.FormaPago> _repository;
    private readonly IMapper _mapper;

    public GetFormaPagoQueryHandler(IRepositoryBase<Domain.Entities.FormaPago> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<(List<FormaPagoListDTO>, int)> Handle(GetFormaPagoQuery req, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable()
            .AsQueryable();

        if (req.Args is null)
        {
            return (_mapper.Map<List<FormaPagoListDTO>>(query.ToList()), query.Count());
        }

        if (!string.IsNullOrEmpty(req.Args.Filter))
        {
            query = query.Where(req.Args.Filter);
        }

        if (!string.IsNullOrEmpty(req.Args.OrderBy))
        {
            query = query.OrderBy(req.Args.OrderBy);
        }

        var skip = req.Args.Skip ?? 0;
        var take = req.Args.Top ?? 15;

        var result = query.Skip(skip).Take(take).ToList();

        return (_mapper.Map<List<FormaPagoListDTO>>(result), result.Count());
    }
}


