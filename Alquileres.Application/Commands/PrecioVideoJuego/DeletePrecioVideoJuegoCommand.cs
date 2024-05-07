using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.PrecioVideoJuego;

public record class DeletePrecioVideoJuegoCommand(int Id) : ICommand<bool>;

internal class DeletePrecioVideoJuegoCommandHandler : ICommandHandler<DeletePrecioVideoJuegoCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.PrecioVideoJuego> _repository;

    public DeletePrecioVideoJuegoCommandHandler(IRepositoryBase<Domain.Entities.PrecioVideoJuego> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeletePrecioVideoJuegoCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entityToDelete == null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }
        entityToDelete.Activado = false;

        await _repository.UpdateAsync(entityToDelete, cancellationToken);
        return true;
    }
}


