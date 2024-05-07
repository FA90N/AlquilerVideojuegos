using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.LineaAlquiler;

public record DeleteLineaAlquilerCommand(int Id) : ICommand<bool>;
internal class DeleteLineaAlquilerCommandHandler : ICommandHandler<DeleteLineaAlquilerCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.LineasAlquiler> _repository;

    public DeleteLineaAlquilerCommandHandler(IRepositoryBase<Domain.Entities.LineasAlquiler> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteLineaAlquilerCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entityToDelete is null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }
        await _repository.DeleteAsync(entityToDelete, cancellationToken);
        return true;
    }
}
