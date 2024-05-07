using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Alquiler;
using Alquileres.Application.Queries.Cliente;
using Alquileres.Application.Queries.LineaAlquiler;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Alquileres.Components.Pages.Alquiler;

public partial class AlquilerDetalle : BaseComponent
{
    [Parameter]
    public int AlquilerId { get; set; }

    public AlquilerFormDTO Alquiler { get; set; } = new();
    public ClienteFormDTO Cliente { get; set; } = new();
    public IList<LineaAlquilerDTO> Lineas { get; set; } = [];

    public bool botonDescargar { get; set; } = false;
    public decimal Importe { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        try
        {

            Alquiler = await Mediator.Send(new GetAlquilerByIdQuery(AlquilerId));
            Cliente = await Mediator.Send(new GetClienteByIdQuery(Alquiler.IdCliente));
            await LoadLinesData();
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }
    async Task LoadLinesData()
    {
        try
        {
            var result = await Mediator.Send(new GetLineaAlquilerQuery(AlquilerId));
            Lineas = result.ToList();
            if (result.Count > 0)
            {
                botonDescargar = true;
                Importe = Alquiler.Dias * Lineas.Sum(x => x.Cantidad * x.Precio);
            }

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }


    void EditarAlquiler()
    {
        NavigationManager.NavigateTo($"alquiler/edit/{AlquilerId}", true);
    }

    async Task Imprimir()
    {
        if (AlquilerId == 0) return;

        try
        {
            var fileName = $"Alquiler_{AlquilerId}_{DateTime.Now:ddMMyyyyHHmmss}.pdf";
            var data = await Mediator.Send(new GetAlquilerReportQuery(AlquilerId));

            await DialogService.OpenAsync<PdfViewer>(fileName,
               new Dictionary<string, object>() { { "Base64data", Convert.ToBase64String(data) } },
               new DialogOptions() { Width = "1024px", Height = "800px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }

    }
}
