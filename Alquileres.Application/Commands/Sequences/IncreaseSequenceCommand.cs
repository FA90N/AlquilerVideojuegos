using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Queries.Sequences;
using MediatR;

namespace Alquileres.Application.Commands.Sequences;

public record IncreaseSequenceCommand(string EntityName, int? Year = null) : ICommand<bool>;

public class IncreaseSequenceCommandHandler : ICommandHandler<IncreaseSequenceCommand, bool>
{
    private readonly IRepositoryBase<Alquileres.Domain.Entities.Sequences> _repository;
    private readonly IMediator _mediator;

    public IncreaseSequenceCommandHandler(IRepositoryBase<Alquileres.Domain.Entities.Sequences> repository, IMediator mediator)
    {
        _repository = repository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(IncreaseSequenceCommand request, CancellationToken cancellationToken)
    {
        var sequence = await _mediator.Send(new GetSequencesByEntityNameQuery(request.EntityName, request.Year), cancellationToken);
        var entityToUpdate = await _repository.GetByIdAsync(sequence!.Id, cancellationToken);

        entityToUpdate.LastNumber++;
        entityToUpdate.LastNumberFormat = entityToUpdate.LastNumber.ToString().PadLeft(5, '0');

        await _repository.UpdateAsync(entityToUpdate);

        return entityToUpdate.Id > 0;
    }
}
