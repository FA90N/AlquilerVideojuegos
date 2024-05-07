using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.VideoJuego;

public record UpdateVideoJuegoCommand : VideoJuegoFormDTO, ICommand<VideoJuegoFormDTO>
{
    public UpdateVideoJuegoCommand(VideoJuegoFormDTO juego) : base(juego)
    {
    }
}

internal class UpdateVideoJuegoCommandHandler : ICommandHandler<UpdateVideoJuegoCommand, VideoJuegoFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    private readonly IAzureStorageService _azureStorageService;

    public UpdateVideoJuegoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureStorageService azureStorageService)
    {
        _unitOfWork = unitOfWork;

        _mapper = mapper;

        _azureStorageService = azureStorageService;
    }

    public async Task<VideoJuegoFormDTO> Handle(UpdateVideoJuegoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.VideoJuego>();

        var entityToUpdate = await repo.GetByIdAsync(req.Id, cancellationToken);

        _mapper.Map(req, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        if (req.ArrayFileData != null)
        {
            await _azureStorageService.UploadFile($"{req.Id}_{req.Imagen}", "caratulas", req.ArrayFileData);
        }

        return _mapper.Map<VideoJuegoFormDTO>(entityToUpdate);
    }
}

public sealed class UpdateVideoJuegoCommandValidator : AbstractValidator<UpdateVideoJuegoCommand>
{
    public UpdateVideoJuegoCommandValidator()
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
