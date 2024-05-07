using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Commands.PrecioVideoJuego;

public record CreatePrecioVideoJuegoCommand : PrecioVideoJuegoFormDTO, ICommand<int>
{
    public CreatePrecioVideoJuegoCommand(PrecioVideoJuegoFormDTO precio) : base(precio) { }
}
internal class CreatePrecioVideoJuegoCommandHandler : ICommandHandler<CreatePrecioVideoJuegoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePrecioVideoJuegoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreatePrecioVideoJuegoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.PrecioVideoJuego>();
        if (repo.GetQueryable().Any(x => x.IdPlataforma == req.IdPlataforma && x.IdVideoJuego == req.IdVideoJuego))
        {
            throw new ArgumentException("Ya existe un la plataforma con los precios");
        }

        var entityAdd = _mapper.Map<Domain.Entities.PrecioVideoJuego>(req);
        await repo.AddAsync(entityAdd, cancellationToken);
        return entityAdd.Id;
    }
}



