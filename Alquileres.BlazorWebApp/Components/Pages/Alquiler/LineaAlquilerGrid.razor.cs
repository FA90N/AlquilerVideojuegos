using Alquileres.Application.Commands.LineaAlquiler;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.LineaAlquiler;
using Alquileres.Application.Queries.PrecioVideoJuego;
using Alquileres.Application.Queries.VideoJuego;
using Alquileres.Components.Shared;
using Alquileres.Helpers;

using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Alquiler;

public partial class LineaAlquilerGrid : BaseComponent
{
    [Parameter]
    public int AlquilerId { get; set; }
    [Parameter]
    public int LineaAlquilerId { get; set; }

    public LineaAlquilerFormDTO Model { get; set; } = new();

    public IList<DropDownModel> VideoJuegos { get; set; } = new List<DropDownModel>();

    public int VideojuegoSelect { get; set; }

    public IList<DropDownModel> PreciosVideojuego { get; set; } = new List<DropDownModel>();

    public List<PrecioVideoJuegoListDTO> ListaPrecios { get; set; } = [];

    public int PrecioSelectId { get; set; }

    public decimal Total { get; set; }

    public PrecioVideoJuegoFormDTO precioJuego { get; set; } = new PrecioVideoJuegoFormDTO()
    {
        Precio = 0,
    };

    public bool IsNew { get; set; }

    public bool Disable { get; set; } = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (LineaAlquilerId != 0)
            {
                Model = await Mediator.Send(new GetLineaAlquilerByIdQuery(LineaAlquilerId));

                var Id = await Mediator.Send(new GetPrecioVideoJuegoByIdQuery(Model.IdPrecioVideojuego));

                VideojuegoSelect = Id.IdVideoJuego;

                var precios = await Mediator.Send(new GetPrecioVideoJuegoQuery(VideojuegoSelect));

                PreciosVideojuego = precios.Select(x => new DropDownModel(x)).ToList();

                ListaPrecios = precios.ToList();

                PrecioSelectId = Model.IdPrecioVideojuego;

                Disable = false;

                precioJuego = await Mediator.Send(new GetPrecioVideoJuegoByIdQuery(PrecioSelectId));

                CantidadOnChange();
            }
            else
            {
                Model = new()
                {
                    Cantidad = 1,
                    IdAlquiler = AlquilerId
                };

            }
            IsNew = LineaAlquilerId == 0;
            var juegos = await Mediator.Send(new GetVideoJuegoQuery());
            VideoJuegos = juegos.Item1.Select(x => new DropDownModel(x)).ToList();

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }

    }

    //Selecciono un juego y aparece un listado de precios con esos datos
    async Task VideoJuegoChange()
    {
        Model.Cantidad = 1;

        precioJuego = new PrecioVideoJuegoFormDTO
        {
            Precio = 0,
        };
        ListaPrecios = [];
        Disable = true;
        Total = 0;
        PrecioSelectId = 0;

        var precios = await Mediator.Send(new GetPrecioVideoJuegoQuery(VideojuegoSelect));
        if (precios.Count > 0)
        {
            Disable = false;
            PreciosVideojuego = precios.Select(x => new DropDownModel(x)).ToList();
            ListaPrecios = precios.ToList();
        }


    }

    //Selecciono la plataforma con precio y me rellena las casillas de precio y calculo el importe por unidades,normalmente 1
    async Task PrecioSelecOnChange()
    {
        precioJuego = await Mediator.Send(new GetPrecioVideoJuegoByIdQuery(PrecioSelectId));

        CantidadOnChange();
    }

    void CantidadOnChange()
    {
        Total = (decimal)(precioJuego.Precio * Model.Cantidad);
    }

    async Task OnSubmit(LineaAlquilerFormDTO linea)
    {
        linea.IdPrecioVideojuego = PrecioSelectId;
        try
        {
            if (LineaAlquilerId != 0)
            {
                await Mediator.Send(new UpdateLineaAlquilerCommand(linea));
            }
            else
            {
                await Mediator.Send(new CreateLineaAlquilerCommand(linea));
            }
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
        NotificationService.Notify(new NotificationMessage
        {
            Severity = NotificationSeverity.Success,
            Summary = "Éxito!",
            Detail = "Los datos han sido guardados satisfactoriamente",
            Duration = 4000
        });

        DialogService.Close(true);


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
