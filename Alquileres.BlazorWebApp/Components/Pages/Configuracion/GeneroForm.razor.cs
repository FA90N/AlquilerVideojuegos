using Alquileres.Application.Commands.Genero;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.Genero;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Configuracion;

public partial class GeneroForm : BaseComponent
{
    [Parameter]
    public int GeneroId { get; set; }

    public GeneroFormDTO Model { get; set; } = new();

    public bool IsNew { get; set; }

    protected override async Task OnInitializedAsync()
    {

        try
        {
            if (GeneroId != 0)
            {
                Model = await Mediator.Send(new GetGeneroByIdQuery(GeneroId));
            }
            else
            {
                Model = new()
                {
                    Activado = true,
                };
            }
            IsNew = GeneroId == 0;
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }

    }

    async Task OnSubmit(GeneroFormDTO genero)
    {
        try
        {
            if (!IsNew)
            {
                await Mediator.Send(new UpdateGeneroCommand(genero));
            }
            else
            {
                GeneroId = await Mediator.Send(new CreateGeneroCommand(genero));
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
