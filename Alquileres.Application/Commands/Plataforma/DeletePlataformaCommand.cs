using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;

namespace Alquileres.Application.Commands.Plataforma;

public record DeletePlataformaCommand(int Id) : ICommand<bool>;
internal class DeletePlataformaCommandHandler : ICommandHandler<DeletePlataformaCommand, bool>
{
    private readonly IRepositoryBase<Domain.Entities.Plataforma> _repository;

    public DeletePlataformaCommandHandler(IRepositoryBase<Domain.Entities.Plataforma> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeletePlataformaCommand req, CancellationToken cancellationToken)
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


