using Alquileres.Application.Commands.VideoJuego;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.VideoJuego;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Radzen;

namespace Alquileres.Components.Pages.VideoJuego;

public partial class VideoJuegoGrid : BaseComponent
{
    public int TotalCount { get; set; }

    public IEnumerable<VideoJuegoListDTO> Data { get; set; } = null;

    public IList<VideoJuegoListDTO> SelectedItems { get; set; } = new List<VideoJuegoListDTO>();

    SharedGridData<VideoJuegoListDTO> grid;

    async Task LoadData(LoadDataArgs args = null)
    {
        try
        {
            var result = await Mediator.Send(new GetVideoJuegoQuery(args));

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
    async Task OnCreateVideoJuego()
    {
        base.NavigationManager.NavigateTo("/videojuego/add", true);
    }
    void OnEditVideoJuego(VideoJuegoListDTO j)
    {
        NavigationManager.NavigateTo($"/videojuego/edit/{j.Id}", true);
    }

    async Task OnDeleteVideoJuego(VideoJuegoListDTO j)
    {
        await SweetAlertHelper.ShowDeleteAlertAsync(Swal, async () => await Mediator.Send(new DeleteVideoJuegoCommand(j.Id)));
        await grid.ReloadData();
    }

    async Task OnPreviewRow(VideoJuegoListDTO j)
    {
        await DialogService.OpenAsync<VideoJuegoDetalle>($"Juego: {j.Nombre}",
          new Dictionary<string, object>() { { "VideojuegoId", j.Id } },
          new DialogOptions() { Width = "700px", Height = "800px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });
    }

}
