using Alquileres.Application.Commands.FormaPago;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.FormaPago;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Configuracion;

public partial class FormaPagoForm : BaseComponent
{
    [Parameter]
    public int PagoId { get; set; }
    public FormaPagoFormDTO Model { get; set; } = new();

    public bool IsNew { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (PagoId != 0)
            {
                Model = await Mediator.Send(new GetFormaPagoByIdQuery(PagoId));
            }
            else
            {
                Model = new()
                {
                    Activado = true,
                };
            }
            IsNew = PagoId == 0;
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    async Task OnSubmit(FormaPagoFormDTO pago)
    {
        try
        {
            if (!IsNew)
            {
                await Mediator.Send(new UpdateFormaPagoCommand(pago));
            }
            else
            {
                PagoId = await Mediator.Send(new CreateFormaPagoCommand(pago));
            }

            DialogService.Close(true);

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Éxito!",
                Detail = "Los datos han sido guardados satisfactoriamente",
                Duration = 4000
            });
        }
        catch (ValidationException vex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Se ha producido un error al validar el formulario",
                Detail = string.Join("<br />", vex.Errors.Select(s => string.Join(".", s.Value)).ToList()),
                Duration = 4000
            });
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    void OnInvalidSubmit(FormInvalidSubmitEventArgs args)
    {
        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Error,
            Summary = "Se ha producido un error al validar el formulario",
            Detail = string.Join("<br />", args.Errors.Select(s => s).ToList()),
            Duration = 4000
        });
    }

}
