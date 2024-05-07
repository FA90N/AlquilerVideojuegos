using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;
using FluentValidation;

namespace Alquileres.Application.Commands.FormaPago;

public record CreateFormaPagoCommand : FormaPagoFormDTO, ICommand<int>
{
    public CreateFormaPagoCommand(FormaPagoFormDTO pago) : base(pago)
    {
    }
}
internal class CreateFormaPagoCommandHandler : ICommandHandler<CreateFormaPagoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateFormaPagoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateFormaPagoCommand req, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.FormaPago>();

        var entityAdd = _mapper.Map<Domain.Entities.FormaPago>(req);

        await repo.AddAsync(entityAdd, cancellationToken);

        return entityAdd.Id;
    }
}


public sealed class CreateFormaPagoCommandValidator : AbstractValidator<UpdateFormaPagoCommand>
{
    public CreateFormaPagoCommandValidator()
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
