using Alquileres.Application.Commands.Cliente;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Cliente;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Alquileres.Components.Pages.Cliente;

public partial class ClienteGrid : BaseComponent
{

    public int TotalCount { get; set; }

    public IEnumerable<ClienteListDTO> Data { get; set; } = null!;

    public IList<ClienteListDTO> SelectedItems { get; set; } = new List<ClienteListDTO>();

    SharedGridData<ClienteListDTO> grid;

    async Task LoadData(LoadDataArgs args = null)
    {
        try
        {
            var result = await Mediator.Send(new GetClienteQuery(args));
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

            if (column is not null)
            {
                column.FilterValue = true;
                await grid.ReloadData();
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    void OnCreateCliente()
    {
        NavigationManager.NavigateTo("/cliente/add", true);
    }

    void OnEditCliente(ClienteListDTO c)
    {
        NavigationManager.NavigateTo($"cliente/edit/{c.Id}", true);
    }

    async Task OnDeleteCliente(ClienteListDTO c)
    {
        await SweetAlertHelper.ShowDeleteAlertAsync(Swal, async () => await Mediator.Send(new DeleteClienteCommand(c.Id)));
        await grid.ReloadData();
    }

    async Task PreviewRow(ClienteListDTO c)
    {
        await DialogService.OpenAsync<DetalleCliente>("",
           new Dictionary<string, object>() { { "ClienteId", c.Id } },
           new DialogOptions() { Width = "700px", Height = "800px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
    }

    async Task OnInputFileChange(InputFileChangeEventArgs e)
    {

    }



}
