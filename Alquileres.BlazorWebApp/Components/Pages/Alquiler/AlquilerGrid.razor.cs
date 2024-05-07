using Alquileres.Application.Commands.Alquiler;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Alquiler;
using Alquileres.Application.Queries.LineaAlquiler;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Radzen;

namespace Alquileres.Components.Pages.Alquiler;

public partial class AlquilerGrid : BaseComponent
{
    public int TotalCount { get; set; }

    public IEnumerable<AlquilerListDTO> Data { get; set; } = null!;

    public IList<AlquilerListDTO> SelectedItems { get; set; } = new List<AlquilerListDTO>();

    SharedGridData<AlquilerListDTO> grid;

    async Task LoadData(LoadDataArgs args = null)
    {
        try
        {
            var result = await Mediator.Send(new GetAlquilerQuery(args));
            Data = result.Item1;
            TotalCount = result.Item2;
        }
        catch (Exception ex)
        {

            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var column = grid.ColumnsCollection.FirstOrDefault(x => x.Property == "Active");

            if (column != null)
            {
                column.FilterValue = true;
                await grid.ReloadData();
            }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    void OnCreateAlquiler()
    {
        NavigationManager.NavigateTo("/alquiler/add", true);
    }

    void OnEditAlquiler(AlquilerListDTO a)
    {
        NavigationManager.NavigateTo($"alquiler/edit/{a.Id}", true);
    }

    async Task OnDeleteAlquiler(AlquilerListDTO a)
    {
        //Buscar si tienes alquileres, si tiene pues lo borro y sino le muestro que no
        var alquiler = await Mediator.Send(new GetLineaAlquilerQuery(a.Id));
        if (alquiler.Count > 0)
        {
            await SweetAlertHelper.ShowError(Swal);
        }
        else
        {
            await SweetAlertHelper.ShowDeleteAlertAsync(Swal, async () => await Mediator.Send(new DeleteAlquilerCommand(a.Id)));
            await grid.ReloadData();
        }

    }

    async Task PreviewRow(AlquilerListDTO a)
    {
        await DialogService.OpenAsync<AlquilerDetalle>($"NºAlquiler: {a.Codigo}",
           new Dictionary<string, object>() { { "AlquilerId", a.Id } },
           new DialogOptions() { Width = "700px", Height = "800px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
    }
}
