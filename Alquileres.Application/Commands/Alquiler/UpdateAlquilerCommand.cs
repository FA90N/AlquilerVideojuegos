using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Commands.Alquiler;

public record UpdateAlquilerCommand : AlquilerFormDTO, ICommand<AlquilerFormDTO>
{
    public UpdateAlquilerCommand(AlquilerFormDTO alquiler) : base(alquiler) { }
}
internal class UpdateAlquilerCommandHandle : ICommandHandler<UpdateAlquilerCommand, AlquilerFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAlquilerCommandHandle(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AlquilerFormDTO> Handle(UpdateAlquilerCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Alquiler>();

        var entityToUpdate = await repo.GetByIdAsync(request.Id, cancellationToken);

        _mapper.Map(request, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        return _mapper.Map<AlquilerFormDTO>(entityToUpdate);

    }
}
