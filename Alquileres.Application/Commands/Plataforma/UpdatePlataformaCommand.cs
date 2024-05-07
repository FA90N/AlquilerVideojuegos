using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.Plataforma;

public record UpdatePlataformaCommand : PlataformaFormDTO, ICommand<PlataformaFormDTO>
{
    public UpdatePlataformaCommand(PlataformaFormDTO plataform) : base(plataform) { }
}

internal class UpdatePlataformaCommandHandler : ICommandHandler<UpdatePlataformaCommand, PlataformaFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePlataformaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PlataformaFormDTO> Handle(UpdatePlataformaCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Plataforma>();

        var entityToUpdate = await repo.GetByIdAsync(req.Id, cancellationToken);

        _mapper.Map(req, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        return _mapper.Map<PlataformaFormDTO>(entityToUpdate);
    }


}

public sealed class UpdatePlataformaCommandValidator : AbstractValidator<UpdatePlataformaCommand>
{
    public UpdatePlataformaCommandValidator()
    {
        const string maxLengthText = "El campo {0} no puede contener más de {1} caracteres";
        const string notEmptyText = "El campo {0} es requerido";
        RuleFor(p => p.Nombre)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "Plataforma"))
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Plataforma", 50));

        RuleFor(p => p.Company)
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Compañia", 50));

        RuleFor(p => p.Version)
           .MaximumLength(30)
           .WithMessage(string.Format(maxLengthText, "Version", 30));


    }
}
