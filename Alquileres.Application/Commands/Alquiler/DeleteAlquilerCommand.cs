using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.Alquiler;

public record DeleteAlquilerCommand(int Id) : ICommand<bool>;
internal class DeleteAlquilerCommandHandle : ICommandHandler<DeleteAlquilerCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.Alquiler> _repository;

    public DeleteAlquilerCommandHandle(IRepositoryBase<Domain.Entities.Alquiler> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteAlquilerCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entityToDelete is null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }

        if (entityToDelete.LineasAlquileres.Where(x => x.IdAlquiler == request.Id).Count() == 0)
        {

            await _repository.DeleteAsync(entityToDelete, cancellationToken);
            return true;

        }

        return false;
    }
}
