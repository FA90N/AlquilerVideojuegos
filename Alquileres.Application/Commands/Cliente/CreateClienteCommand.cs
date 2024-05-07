using Alquileres.Application.Commands.Sequences;
using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.Sequences;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Alquileres.Application.Commands.Cliente;

public record class CreateClienteCommand : ClienteFormDTO, ICommand<int>
{
    public CreateClienteCommand(ClienteFormDTO videocliente) : base(videocliente)
    {
    }
}

internal class CreateClienteCommandHandler : ICommandHandler<CreateClienteCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    private readonly IAzureStorageService _azureStorageService;

    public CreateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator, IAzureStorageService azureStorageService)
    {
        _unitOfWork = unitOfWork;

        _mapper = mapper;

        _mediator = mediator;

        _azureStorageService = azureStorageService;
    }

    public async Task<int> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Cliente>();

        //comprobar que no existe un cliente con el dni proporcionado
        if (repo.GetQueryable().Any(x => x.Dni == request.Dni))
        {
            //throw new DuplicateWaitObjectException("Ya existe un DNI Con esos datos");
            throw new ArgumentException("Ya existe un DNI con esos datos");
        }

        var entityToAdd = _mapper.Map<Domain.Entities.Cliente>(request);

        await _mediator.Send(new IncreaseSequenceCommand(SequencesEntityName.Cliente));

        await repo.AddAsync(entityToAdd, cancellationToken);


        if (request.ArrayFileData != null)
        {
            await _azureStorageService.UploadFile($"{entityToAdd.Id}_{request.Documento}", "usuarios", request.ArrayFileData);
        }

        return entityToAdd.Id;
    }

}
public sealed class CreateClienteCommandValidator : AbstractValidator<CreateClienteCommand>
{
    public CreateClienteCommandValidator()
    {

        const string maxLengthText = "El campo {0} no puede contener más de {1} caracteres";
        const string notEmptyText = "El campo {0} es requerido";
        RuleFor(p => p.Nombre)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "Nombre"))
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Nombre", 50));

        RuleFor(p => p.Apellidos)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "Apellidos"))
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Apellidos", 50));

        RuleFor(p => p.Dni)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "DNI/NIE"))
            .MaximumLength(9)
            .WithMessage(string.Format(maxLengthText, "DNI/NIE", 9));

        RuleFor(p => p.Comentario)
           .MaximumLength(200)
           .WithMessage(string.Format(maxLengthText, "Comentario", 200));

        RuleFor(p => p.Telefono)
          .MaximumLength(15)
          .WithMessage(string.Format(maxLengthText, "Telefono", 15));

        RuleFor(p => p.Email)
          .MaximumLength(50)
          .WithMessage(string.Format(maxLengthText, "Email", 50));

        When(x => x.FechaNacimiento.HasValue, () =>
        {
            RuleFor(p => p).Must(x => x.FechaNacimiento.Value.Date <= DateTime.Now.Date.AddYears(-16))
            .WithMessage("Debe ser mayor de 16 años");
        });
    }
}
