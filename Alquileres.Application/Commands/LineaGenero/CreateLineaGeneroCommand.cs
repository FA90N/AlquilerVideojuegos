using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Queries;
using AutoMapper;

namespace Alquileres.Application.Commands.LineaGenero;

public record class CreateLineaGeneroCommand : LineaGeneroDTO, ICommand<int>
{
    public CreateLineaGeneroCommand(LineaGeneroDTO genero) : base(genero)
    {
    }
}

internal class CreateLineaGeneroCommandHandler : ICommandHandler<CreateLineaGeneroCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateLineaGeneroCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateLineaGeneroCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.LineasGenero>();
        var entityToAdd = _mapper.Map<Domain.Entities.LineasGenero>(req);
        await repo.AddAsync(entityToAdd, cancellationToken);
        return entityToAdd.Id;

    }
}
