using Alquileres.Application.Commands.Plataforma;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.Plataforma;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Configuracion;

public partial class PlataformaForm : BaseComponent
{
    [Parameter]
    public int PlataformaId { get; set; }
    public PlataformaFormDTO Model { get; set; } = new();
    public bool IsNew { get; set; }

    protected override async Task OnInitializedAsync()
    {

        try
        {
            if (PlataformaId != 0)
            {
                Model = await Mediator.Send(new GetPlataformaByIdQuery(PlataformaId));
            }
            else
            {
                Model = new()
                {
                    Activado = true,
                };

            }
            IsNew = PlataformaId == 0;

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }
    async Task OnSubmit(PlataformaFormDTO p)
    {
        try
        {
            if (!IsNew)
            {
                await Mediator.Send(new UpdatePlataformaCommand(p));
            }
            else
            {
                PlataformaId = await Mediator.Send(new CreatePlataformaCommand(p));
            }

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Éxito!",
                Detail = "Los datos han sido guardados satisfactoriamente",
                Duration = 4000
            });

            DialogService.Close(true);

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
