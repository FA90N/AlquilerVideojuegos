using Alquileres.Application.Commands.Alquiler;
using Alquileres.Application.Commands.Cliente;
using Alquileres.Application.Commands.LineaAlquiler;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Alquiler;
using Alquileres.Application.Queries.Cliente;
using Alquileres.Application.Queries.FormaPago;
using Alquileres.Application.Queries.LineaAlquiler;
using Alquileres.Application.Queries.Sequences;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Alquiler;

public partial class AlquilerEdit : BaseComponent
{
    [Parameter]
    public int AlquilerId { get; set; }
    public bool IsNew { get; set; }

    public AlquilerFormDTO Model { get; set; } = new AlquilerFormDTO();

    public IList<DropDownModel> ClienteList { get; set; } = new List<DropDownModel>();

    public IList<DropDownModel> FormaPagoList { get; set; } = new List<DropDownModel>();

    public ClienteFormDTO Cliente { get; set; } = new ClienteFormDTO();

    private bool ActiveCliente { get; set; }

    public bool botonDescargar { get; set; } = false;

    public decimal Importe { get; set; } = 0;

    //Lineas de alquiler
    public IEnumerable<LineaAlquilerDTO> LineasAlquiler { get; set; }

    SharedGridData<LineaAlquilerDTO> gridLines;

    public bool Descargar { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {

            if (AlquilerId != 0)
            {
                Model = await Mediator.Send(new GetAlquilerByIdQuery(AlquilerId));
                Cliente = await Mediator.Send(new GetClienteByIdQuery(Model.IdCliente));
                await LoadLinesData();

            }
            else
            {
                Model = new()
                {
                    Code = (await Mediator.Send(new GetSequencesByEntityNameQuery(SequencesEntityName.Alquiler))).LastNumberFormat,
                    Fecha = DateTime.Now,
                    Dias = 0,
                };
            }
            IsNew = AlquilerId == 0;

            //Obtener la lista de formapago y asignarlo
            var formapago = await Mediator.Send(new GetFormaPagoQuery());
            FormaPagoList = formapago.Item1.Select(x => new DropDownModel(x)).ToList();

            //Obtener la lista de cliente y asignarlo
            var clientes = await Mediator.Send(new GetClienteQuery());
            ClienteList = clientes.Item1.Select(x => new DropDownModel(x)).ToList();

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }

    }
    async Task cambioDias()
    {
        Importe = 0;
        try
        {
            var d = Model.FechaFin.Date - Model.Fecha.Date;
            if (d.Days > 0)
            {
                Model.Dias = d.Days;

                if (LineasAlquiler != null)
                    Importe = Model.Dias * LineasAlquiler.Sum(x => x.Cantidad * x.Precio);
            }
            else
            {
                Model.Dias = 0;
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "La fecha de devolución no puede ser igual o anterior a la fecha de alquiler",
                    Duration = 2000
                });
            }

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }
    async Task ClienteSelected()
    {
        try
        {
            Cliente = await Mediator.Send(new GetClienteByIdQuery(Model.IdCliente));
        }
        catch (Exception ex)
        {

            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }
    private async Task<bool> ActivateCliente(int clienteId)
    {
        ActiveCliente = false;
        var client = await Mediator.Send(new GetClienteByIdQuery(clienteId));

        if (client is not null && !client.Activado)
        {
            await SweetAlertHelper.ShowConfirmAlertAsync(Swal, "¿Desea activar el cliente?", "El cliente seleccionado no está activo en estos momentos", async () =>
            {
                client.Activado = true;
                await Mediator.Send(new UpdateClienteCommand(client));
                ActiveCliente = true;
                return true;
            });
        }
        return (client is not null && client.Activado) || ActiveCliente;
    }



    async Task OnSubmit(AlquilerFormDTO alquiler)
    {
        try
        {
            var continueProcess = await ActivateCliente(alquiler.IdCliente);

            if (continueProcess)
            {
                if (!IsNew)
                {
                    await Mediator.Send(new UpdateAlquilerCommand(alquiler));
                }
                else
                {
                    AlquilerId = await Mediator.Send(new CreateAlquilerCommand(alquiler));
                }
                await gridLines.ReloadData();
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Éxito!",
                    Detail = "Los datos del alquiler han sido guardados satisfactoriamente",
                    Duration = 4000
                });
                if (IsNew)
                {
                    await Task.Delay(1000);
                    NavigationManager.NavigateTo($"/alquiler/edit/{AlquilerId}", true, true);
                }
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Se ha producido un error al validar el formulario",
                    Detail = "El cliente esta desactivado",
                    Duration = 4000
                });
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
    void Cancel()
    {
        NavigationManager.NavigateTo("/alquiler", true, true);
    }

    //Lineas de alquileres
    async Task CreateLine()
    {
        var result = await DialogService.OpenAsync<LineaAlquilerGrid>($"{Model.IdCliente} - {Cliente.Nombre}", new Dictionary<string, object>()
        {
            {"AlquilerId", AlquilerId },
            {"LineaAlquilerId",0 },
        }, DialogServiceHelper.DialogOptionsBuilder());
        if (result is bool && (bool)result)
        {
            await LoadLinesData();
        }

    }

    async Task EditLineRow(LineaAlquilerDTO linea)
    {
        var result = await DialogService.OpenAsync<LineaAlquilerGrid>($"{Model.IdCliente} - {Cliente.Nombre}",
         new Dictionary<string, object>()
        {
            {"AlquilerId", AlquilerId },
            {"LineaAlquilerId",linea.Id },
        }, DialogServiceHelper.DialogOptionsBuilder());

        if (result is bool && (bool)result)
        {
            await LoadLinesData();
        }


    }

    async Task DeleteLineRow(LineaAlquilerDTO linea)
    {
        await SweetAlertHelper.ShowDeleteAlertAsync(Swal, async () => await Mediator.Send(new DeleteLineaAlquilerCommand(linea.Id)));
        await LoadLinesData();
    }

    async Task LoadLinesData()
    {

        try
        {
            var result = await Mediator.Send(new GetLineaAlquilerQuery(AlquilerId));
            LineasAlquiler = result.ToList();
            if (result.Count > 0)
            {
                botonDescargar = true;
                Importe = Model.Dias * LineasAlquiler.Sum(x => x.Cantidad * x.Precio);
            }
            else
            {
                botonDescargar = false;
                Importe = 0;
            }
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }
    async Task Imprimir()
    {
        if (AlquilerId == 0) return;

        try
        {
            Descargar = true;

            var fileName = $"Alquiler_{AlquilerId}_{DateTime.Now:ddMMyyyyHHmmss}.pdf";
            var data = await Mediator.Send(new GetAlquilerReportQuery(AlquilerId));

            Descargar = false;

            await DialogService.OpenAsync<PdfViewer>(fileName,
               new Dictionary<string, object>() { { "Base64data", Convert.ToBase64String(data) } },
               new DialogOptions() { Width = "1024px", Height = "800px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
        finally
        {
            Descargar = false;
        }
    }
}
