using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.FormaPago;

public record DeleteFormaPagoCommand(int Id) : ICommand<bool>;
internal class DeleteFormaPagoCommandHandler : ICommandHandler<DeleteFormaPagoCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.FormaPago> _repository;

    public DeleteFormaPagoCommandHandler(IRepositoryBase<Domain.Entities.FormaPago> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFormaPagoCommand req, CancellationToken cancellationToken)
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
