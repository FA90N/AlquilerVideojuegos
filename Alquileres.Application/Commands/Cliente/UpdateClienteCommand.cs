using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.Cliente;

public record UpdateClienteCommand : ClienteFormDTO, ICommand<ClienteFormDTO>
{
    public UpdateClienteCommand(ClienteFormDTO cliente) : base(cliente)
    {
    }
}
internal class UpdateClienteCommandHandler : ICommandHandler<UpdateClienteCommand, ClienteFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    private readonly IAzureStorageService _azureStorageService;


    public UpdateClienteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureStorageService azureStorageService)
    {
        _unitOfWork = unitOfWork;

        _mapper = mapper;

        _azureStorageService = azureStorageService;

    }

    public async Task<ClienteFormDTO> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {


        var repo = _unitOfWork.Repository<Domain.Entities.Cliente>();

        if (repo.GetQueryable().Any(x => x.Dni == request.Dni && x.Id != request.Id))
        {
            throw new ArgumentException("Ya existe un DNI con esos datos");
        }

        var entityToUpdate = await repo.GetByIdAsync(request.Id, cancellationToken);

        _mapper.Map(request, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        if (request.ArrayFileData != null)
        {
            await _azureStorageService.UploadFile($"{request.Id}_{request.Documento}", "usuarios", request.ArrayFileData);
        }
        return _mapper.Map<ClienteFormDTO>(entityToUpdate);
    }

}

public sealed class UpdateClienteCommandValidator : AbstractValidator<UpdateClienteCommand>
{
    public UpdateClienteCommandValidator()
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
            .MaximumLength(50)
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


