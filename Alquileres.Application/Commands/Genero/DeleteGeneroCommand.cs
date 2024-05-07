using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.Genero;

public record DeleteGeneroCommand(int Id) : ICommand<bool>;

internal class DeleteGeneroCommandHandler : ICommandHandler<DeleteGeneroCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.Genero> _repository;

    public DeleteGeneroCommandHandler(IRepositoryBase<Domain.Entities.Genero> repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(DeleteGeneroCommand req, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(req.Id, cancellationToken);
        if (entityToDelete == null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }

        entityToDelete.Activado = false;
        await _repository.UpdateAsync(entityToDelete, cancellationToken);
        return true;
    }
}
