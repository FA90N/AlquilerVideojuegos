using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.VideoJuego;

public record DeleteVideoJuegoCommand(int Id) : ICommand<bool>;

internal class DeleteVideoJuegoCommandHandler : ICommandHandler<DeleteVideoJuegoCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.VideoJuego> _repository;

    public DeleteVideoJuegoCommandHandler(IRepositoryBase<Domain.Entities.VideoJuego> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteVideoJuegoCommand req, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(req.Id, cancellationToken);
        if (entityToDelete is null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }

        entityToDelete.Activado = false;
        await _repository.UpdateAsync(entityToDelete, cancellationToken);
        return true;
    }
}

