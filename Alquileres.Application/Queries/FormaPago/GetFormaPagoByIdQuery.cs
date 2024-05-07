using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.FormaPago;

public record GetFormaPagoByIdQuery(int Id) : IQuery<FormaPagoFormDTO>;
internal class GetFormaPagoByIdQueryHandler : IQueryHandler<GetFormaPagoByIdQuery, FormaPagoFormDTO>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.FormaPago> _repository;

    private readonly IMapper _mapper;

    public GetFormaPagoByIdQueryHandler(IRepositoryBase<Alquileres.Domain.Entities.FormaPago> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FormaPagoFormDTO> Handle(GetFormaPagoByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id, cancellationToken);

        return _mapper.Map<FormaPagoFormDTO>(result);
    }
}
