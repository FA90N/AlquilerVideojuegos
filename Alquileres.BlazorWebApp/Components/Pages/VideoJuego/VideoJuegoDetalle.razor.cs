using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.PrecioVideoJuego;
using Alquileres.Application.Queries.VideoJuego;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;

namespace Alquileres.Components.Pages.VideoJuego;

public partial class VideoJuegoDetalle : BaseComponent
{
    [Parameter]
    public int VideojuegoId { get; set; }

    public VideoJuegoFormDTO VideoJuego { get; set; } = new();

    public List<PrecioVideoJuegoListDTO> ListaPrecio { get; set; } = new();

    string fileData = "img/juego.png";


    protected override async Task OnInitializedAsync()
    {
        try
        {
            VideoJuego = await Mediator.Send(new GetVideoJuegoByIdQuery(VideojuegoId));


            ListaPrecio = await Mediator.Send(new GetPrecioVideoJuegoQuery(VideojuegoId));

            if (VideoJuego.Fichero != null)
            {
                fileData = $"data:{VideoJuego.Imagen.Split(".")[1]};base64,{VideoJuego.Fichero}";
            }
        }
        catch (Exception ex)
        {

            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }


    }

    async Task EditVideojuego()
    {
        NavigationManager.NavigateTo($"/videojuego/edit/{VideojuegoId}", true);
    }
}
