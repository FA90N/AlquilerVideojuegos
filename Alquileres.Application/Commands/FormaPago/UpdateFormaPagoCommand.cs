using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.FormaPago;

public record UpdateFormaPagoCommand : FormaPagoFormDTO, ICommand<FormaPagoFormDTO>
{
    public UpdateFormaPagoCommand(FormaPagoFormDTO pago) : base(pago)
    {
    }
}
internal class UpdateFormaPagoCommandHanlder : ICommandHandler<UpdateFormaPagoCommand, FormaPagoFormDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UpdateFormaPagoCommandHanlder(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FormaPagoFormDTO> Handle(UpdateFormaPagoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.FormaPago>();

        var entityToUpdate = await repo.GetByIdAsync(req.Id, cancellationToken);

        _mapper.Map(req, entityToUpdate);

        await repo.UpdateAsync(entityToUpdate, cancellationToken);

        return _mapper.Map<FormaPagoFormDTO>(entityToUpdate);
    }
}

public sealed class UpdateFormaPagoCommandValidator : AbstractValidator<UpdateFormaPagoCommand>
{
    public UpdateFormaPagoCommandValidator()
    {
        const string maxLengthText = "El campo {0} no puede contener más de {1} caracteres";
        const string notEmptyText = "El campo {0} es requerido";

        RuleFor(p => p.Nombre)
        .NotEmpty()
        .WithMessage(string.Format(notEmptyText, "Forma de pago"))
        .MaximumLength(50)
        .WithMessage(string.Format(maxLengthText, "Forma de pago", 50));


        RuleFor(p => p.Descripcion)
        .MaximumLength(256)
        .WithMessage(string.Format(maxLengthText, "Descripción", 256));
    }
}
