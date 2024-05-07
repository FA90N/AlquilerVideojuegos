using Alquileres.Application.Commands.PrecioVideoJuego;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Plataforma;
using Alquileres.Application.Queries.PrecioVideoJuego;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.VideoJuego;

public partial class PrecioVideoJuegoEdit : BaseComponent
{
    [Parameter]
    public int VideojuegoId { get; set; }

    [Parameter]
    public int PreciovideojuegoId { get; set; }

    public PrecioVideoJuegoFormDTO Model { get; set; } = new();

    public IList<DropDownModel> Plataformas { get; set; } = new List<DropDownModel>();

    public bool IsNew { get; set; }

    protected override async Task OnInitializedAsync()
    {

        try
        {
            if (PreciovideojuegoId != 0)
            {
                Model = await Mediator.Send(new GetPrecioVideoJuegoByIdQuery(PreciovideojuegoId));
            }
            else
            {
                Model = new()
                {
                    Activado = true,
                };
            }

            IsNew = PreciovideojuegoId == 0;
            var plataforma = await Mediator.Send(new GetPlataformaQuery());
            Plataformas = plataforma.Item1.Select(x => new DropDownModel(x)).ToList();
            Model.IdVideoJuego = VideojuegoId;

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

    async Task OnSubmit()
    {


        try
        {
            if (PreciovideojuegoId != 0)
            {
                //Actualizar
                await Mediator.Send(new UpdatePrecioVideoJuegoCommand(Model));
            }
            else
            {
                //Crear
                await Mediator.Send(new CreatePrecioVideoJuegoCommand(Model));
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

    bool Validate(int IdPlataforma)
    {
        var res = Mediator.Send(new GetPrecioVideoJuegoByIdPlatformQuery(IdPlataforma, VideojuegoId));
        if (res.Result != null)
        {
            return true;
        }

        return false;
    }
}
