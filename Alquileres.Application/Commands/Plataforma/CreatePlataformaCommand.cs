using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.Plataforma;

public record CreatePlataformaCommand : PlataformaFormDTO, ICommand<int>
{
    public CreatePlataformaCommand(PlataformaFormDTO plataforma) : base(plataforma)
    {
    }
}
internal class CreatePlataformaCommandHandler : ICommandHandler<CreatePlataformaCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePlataformaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreatePlataformaCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Plataforma>();
        var entityAdd = _mapper.Map<Domain.Entities.Plataforma>(req);
        await repo.AddAsync(entityAdd, cancellationToken);
        return entityAdd.Id;
    }
}

public sealed class CreatePlataformaCommandValidator : AbstractValidator<CreatePlataformaCommand>
{
    public CreatePlataformaCommandValidator()
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

