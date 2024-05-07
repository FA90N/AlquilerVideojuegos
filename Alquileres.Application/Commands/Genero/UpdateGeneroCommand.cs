using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.Genero;

public record UpdateGeneroCommand : GeneroFormDTO, ICommand<GeneroFormDTO>
{
    public UpdateGeneroCommand(GeneroFormDTO genero) : base(genero) { }
}
internal class UpdateGeneroCommandHandler : ICommandHandler<UpdateGeneroCommand, GeneroFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateGeneroCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GeneroFormDTO> Handle(UpdateGeneroCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Genero>();

        var entityToUpdate = await repo.GetByIdAsync(req.Id, cancellationToken);

        _mapper.Map(req, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        return _mapper.Map<GeneroFormDTO>(entityToUpdate);
    }

}

public sealed class UpdateGeneroCommandValidator : AbstractValidator<UpdateGeneroCommand>
{
    public UpdateGeneroCommandValidator()
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