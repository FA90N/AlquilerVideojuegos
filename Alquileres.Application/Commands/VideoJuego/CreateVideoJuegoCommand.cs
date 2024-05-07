using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.VideoJuego;

public record class CreateVideoJuegoCommand : VideoJuegoFormDTO, ICommand<int>
{
    public CreateVideoJuegoCommand(VideoJuegoFormDTO juego) : base(juego)
    {
    }
}

internal class CreateVideoJuegoCommandHandler : ICommandHandler<CreateVideoJuegoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    private readonly IAzureStorageService _azureStorageService;


    public CreateVideoJuegoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureStorageService azureStorageService)
    {
        _unitOfWork = unitOfWork;

        _mapper = mapper;

        _azureStorageService = azureStorageService;

    }

    public async Task<int> Handle(CreateVideoJuegoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.VideoJuego>();

        var entityAdd = _mapper.Map<Domain.Entities.VideoJuego>(req);

        await repo.AddAsync(entityAdd, cancellationToken);
        if (req.ArrayFileData != null)
        {
            await _azureStorageService.UploadFile($"{entityAdd.Id}_{req.Imagen}", "caratulas", req.ArrayFileData);
        }

        return entityAdd.Id;
    }
}

public sealed class CreateVideoJuegoCommandValidator : AbstractValidator<CreateVideoJuegoCommand>
{
    public CreateVideoJuegoCommandValidator()
    {
        const string maxLengthText = "El campo {0} no puede contener más de {1} caracteres";
        const string notEmptyText = "El campo {0} es requerido";

        RuleFor(p => p.Nombre)
            .NotEmpty()
            .WithMessage(string.Format(notEmptyText, "Nombre"))
            .MaximumLength(50)
            .WithMessage(string.Format(maxLengthText, "Nombre", 50));

        RuleFor(p => p.Descripcion)
          .MaximumLength(256)
          .WithMessage(string.Format(maxLengthText, "Descripción", 256));

        RuleFor(p => p.Desarrollador)
          .MaximumLength(50)
          .WithMessage(string.Format(maxLengthText, "Desarrollador", 50));

        RuleFor(p => p.Pegi)
           .MaximumLength(50)
          .WithMessage(string.Format(maxLengthText, "Pegi", 50));



    }
}
