using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Commands.LineaAlquiler;

public record UpdateLineaAlquilerCommand : LineaAlquilerFormDTO, ICommand<LineaAlquilerFormDTO>
{
    public UpdateLineaAlquilerCommand(LineaAlquilerFormDTO linea) : base(linea) { }
}
internal class UpdateLineaAlquilerCommandHandler : ICommandHandler<UpdateLineaAlquilerCommand, LineaAlquilerFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public UpdateLineaAlquilerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<LineaAlquilerFormDTO> Handle(UpdateLineaAlquilerCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.LineasAlquiler>();

        var entityToUpdate = await repo.GetByIdAsync(request.Id, cancellationToken);

        _mapper.Map(request, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        return _mapper.Map<LineaAlquilerFormDTO>(entityToUpdate);
    }
}
