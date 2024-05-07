using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Commands.PrecioVideoJuego;

public record UpdatePrecioVideoJuegoCommand : PrecioVideoJuegoFormDTO, ICommand<PrecioVideoJuegoFormDTO>
{
    public UpdatePrecioVideoJuegoCommand(PrecioVideoJuegoFormDTO precio) : base(precio)
    {
    }
}

internal class UpdatePrecioVideoJuegoCommandHandler : ICommandHandler<UpdatePrecioVideoJuegoCommand, PrecioVideoJuegoFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePrecioVideoJuegoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PrecioVideoJuegoFormDTO> Handle(UpdatePrecioVideoJuegoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.PrecioVideoJuego>();
        var entityToUpdate = await repo.GetByIdAsync(req.Id, cancellationToken);
        _mapper.Map(req, entityToUpdate);
        await repo.UpdateAsync(entityToUpdate, cancellationToken);
        return _mapper.Map<PrecioVideoJuegoFormDTO>(entityToUpdate);
    }
}


