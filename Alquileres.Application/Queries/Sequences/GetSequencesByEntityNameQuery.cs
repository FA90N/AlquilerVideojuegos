using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Alquileres.Application.Queries.Sequences;

public struct SequencesEntityName
{

    public const string Cliente = "Cliente";
    public const string Alquiler = "Alquiler";
}

public record GetSequencesByEntityNameQuery(string EntityName, int? Year = null) : IQuery<SequencesDTO>;

public class GetSequencesByEntityNameQueryHandler : IQueryHandler<GetSequencesByEntityNameQuery, SequencesDTO>
{
    private readonly IRepositoryBase<Domain.Entities.Sequences> _repository;
    private readonly IMapper _mapper;

    public GetSequencesByEntityNameQueryHandler(IRepositoryBase<Domain.Entities.Sequences> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SequencesDTO> Handle(GetSequencesByEntityNameQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQueryable(w => w.EntityName == request.EntityName);

        if (request.Year.HasValue)
        {
            query = query.Where(x => x.Year == request.Year);
        }

        var result = await query.FirstOrDefaultAsync();

        // Si no existe la secuencia la creamos
        if (result is null)
        {
            await _repository.AddAsync(new Domain.Entities.Sequences
            {
                EntityName = request.EntityName,
                LastNumber = 1,
                LastNumberFormat = "00001",
                ResetYear = true,
                Year = request.Year
            }, cancellationToken);

            result = await _repository
                .GetQueryable(w => w.EntityName == request.EntityName && w.Year == request.Year)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        return _mapper.Map<SequencesDTO>(result);
    }
}
