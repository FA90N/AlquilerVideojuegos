using Alquileres.Application.Commands.Sequences;
using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.Sequences;
using AutoMapper;
using MediatR;

namespace Alquileres.Application.Commands.Alquiler;

public record class CreateAlquilerCommand : AlquilerFormDTO, ICommand<int>
{
    public CreateAlquilerCommand(AlquilerFormDTO alquiler) : base(alquiler)
    {
    }
}

internal class CreateAlquilerCommandHandler : ICommandHandler<CreateAlquilerCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateAlquilerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<int> Handle(CreateAlquilerCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Alquiler>();

        var entityToAdd = _mapper.Map<Domain.Entities.Alquiler>(request);

        await _mediator.Send(new IncreaseSequenceCommand(SequencesEntityName.Alquiler));

        await repo.AddAsync(entityToAdd, cancellationToken);

        return entityToAdd.Id;
    }
}

