using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.Cliente;

public record DeleteClienteCommand(int Id) : ICommand<bool>;
internal class DeleteClienteCommandHandler : ICommandHandler<DeleteClienteCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.Cliente> _repository;

    public DeleteClienteCommandHandler(IRepositoryBase<Domain.Entities.Cliente> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        var entityToDelete = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entityToDelete is null)
        {
            throw new Exception("No existe el elemento seleccionado");
        }

        entityToDelete.Activado = false;
        await _repository.UpdateAsync(entityToDelete, cancellationToken);
        return true;
    }


}

