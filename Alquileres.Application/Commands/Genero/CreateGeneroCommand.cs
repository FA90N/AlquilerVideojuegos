using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.Genero;

public record CreateGeneroCommand : GeneroFormDTO, ICommand<int>
{
    public CreateGeneroCommand(GeneroFormDTO video) : base(video)
    {
    }
}

internal class CreateGeneroCommandHandler : ICommandHandler<CreateGeneroCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateGeneroCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateGeneroCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Genero>();
        var entityAdd = _mapper.Map<Domain.Entities.Genero>(request);
        await repo.AddAsync(entityAdd, cancellationToken);
        return entityAdd.Id;
    }
}

public sealed class CreateGeneroCommmandValidator : AbstractValidator<CreateGeneroCommand>
{
    public CreateGeneroCommmandValidator()
    {
        const string maxLengthText = "El campo {0} no puede contener más de {1} caracteres";
        const string notEmptyText = "El campo {0} es requerido";
        RuleFor(p => p.Nombre)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "Género"))
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Género", 50));

    }
}


