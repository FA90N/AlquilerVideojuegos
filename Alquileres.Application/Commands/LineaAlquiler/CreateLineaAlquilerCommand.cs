using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Commands.LineaAlquiler;

public record class CreateLineaAlquilerCommand : LineaAlquilerFormDTO, ICommand<int>
{
    public CreateLineaAlquilerCommand(LineaAlquilerFormDTO linea) : base(linea) { }

}
internal class CreateLineaAlquilerCommandHandler : ICommandHandler<CreateLineaAlquilerCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public CreateLineaAlquilerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateLineaAlquilerCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.LineasAlquiler>();

        var entityToAdd = _mapper.Map<Domain.Entities.LineasAlquiler>(request);

        await repo.AddAsync(entityToAdd, cancellationToken);

        return entityToAdd.Id;
    }
}

