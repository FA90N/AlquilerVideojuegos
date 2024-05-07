using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;

namespace Alquileres.Application.Commands.LineaGenero;

public record UpdateLineaGeneroCommand(int VideojuegoId, IList<int> Generos) : ICommand<bool>;

internal class UpdateLineaGeneroCommandHandler : ICommandHandler<UpdateLineaGeneroCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateLineaGeneroCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateLineaGeneroCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.LineasGenero>();
        var query = repo.GetQueryable().Where(x => x.IdVideojuego == req.VideojuegoId).ToList();
        foreach (var q in query)
        {
            await repo.DeleteAsync(q, cancellationToken);
        }

        foreach (var r in req.Generos)
        {
            var entityToUpdate = await repo.AddAsync(new Domain.Entities.LineasGenero
            {
                IdGenero = r,
                IdVideojuego = req.VideojuegoId,

            }, cancellationToken);
        }


        return true;
    }
}