using Alquileres.Application.Commands.LineaGenero;
using Alquileres.Application.Commands.PrecioVideoJuego;
using Alquileres.Application.Commands.VideoJuego;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Genero;
using Alquileres.Application.Queries.LineaGenero;
using Alquileres.Application.Queries.PrecioVideoJuego;
using Alquileres.Application.Queries.VideoJuego;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Alquileres.Components.Pages.VideoJuego;

public partial class VideoJuegoEdit : BaseComponent
{
    [Parameter]
    public int VideoJuegoId { get; set; }

    public VideoJuegoFormDTO Model { get; set; } = new();

    public bool IsNew { get; set; }

    public IList<DropDownModel> Generos { get; set; }

    public IList<int> GenerosSeleccionados { get; set; }

    public IEnumerable<PrecioVideoJuegoListDTO> Precios { get; set; } = null!;

    SharedGridData<PrecioVideoJuegoListDTO> gridPrecio;

    string fileData;


    protected override async Task OnInitializedAsync()
    {
        try
        {
            if (VideoJuegoId != 0)
            {
                Model = await Mediator.Send(new GetVideoJuegoByIdQuery(VideoJuegoId));
                await LoadLinesData();
            }
            else
            {
                Model = new()
                {
                    Activado = true,
                };

            }

            IsNew = VideoJuegoId == 0;

            var generos = await Mediator.Send(new GetGeneroQuery());

            Generos = generos.Item1.Select(x => new DropDownModel(x)).ToList();

            var generosSelect = await Mediator.Send(new GetLineaGeneroByIdQuery(VideoJuegoId));

            GenerosSeleccionados = generosSelect.Select(x => x.IdGenero).ToList();
            if (Model.Fichero != null)
            {
                fileData = $"data:{Model.Imagen.Split(".")[1]};base64,{Model.Fichero}";
            }

        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    async Task OnSubmit(VideoJuegoFormDTO juego)
    {
        try
        {

            if (!IsNew)
            {
                await Mediator.Send(new UpdateVideoJuegoCommand(juego));

                await Mediator.Send(new UpdateLineaGeneroCommand(VideoJuegoId, GenerosSeleccionados));

            }
            else
            {
                VideoJuegoId = await Mediator.Send(new CreateVideoJuegoCommand(juego));

                foreach (var item in GenerosSeleccionados)
                {
                    var linea = new LineaGeneroDTO
                    {
                        IdVideojuego = VideoJuegoId,
                        IdGenero = item
                    };


                    await Mediator.Send(new CreateLineaGeneroCommand(linea));
                }
            }

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Éxito!",
                Detail = "Los datos del videojuego han sido guardados satisfactoriamente",
                Duration = 4000
            });
            if (IsNew)
            {
                await Task.Delay(1000);
                NavigationManager.NavigateTo($"/videojuego/edit/{VideoJuegoId}", true, true);
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
        NavigationManager.NavigateTo("/videojuego", true, true);
    }
    //Listado de precios dentro del formulario
    async Task CreateLine()
    {
        var result = await DialogService.OpenAsync<PrecioVideoJuegoEdit>(
            "Precio por plataforma", new Dictionary<string, object>()
        {
            {"VideoJuegoId", VideoJuegoId },
            {"PreciovideojuegoId",0 }
        }, new DialogOptions() { Width = "700px", Height = "400px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });


        if (result is bool && (bool)result)
        {
            await LoadLinesData();
        }
    }

    async Task EditLineRow(PrecioVideoJuegoListDTO linea)
    {

        var result = await DialogService.OpenAsync<PrecioVideoJuegoEdit>("Precio por plataforma", new Dictionary<string, object>()
        {
            {"VideoJuegoId", VideoJuegoId },
            {"PreciovideojuegoId",linea.Id }
        }, new DialogOptions() { Width = "700px", Height = "400px", Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true });

        if (result is bool && (bool)result)
        {
            await LoadLinesData();
        }
    }

    async Task DeleteLineRow(PrecioVideoJuegoListDTO linea)
    {

        await SweetAlertHelper.ShowDeleteAlertAsync(Swal, async () => await Mediator.Send(new DeletePrecioVideoJuegoCommand(linea.Id)));
        await LoadLinesData();
    }

    async Task LoadLinesData()
    {
        try
        {
            var res = await Mediator.Send(new GetPrecioVideoJuegoQuery(VideoJuegoId, null));
            Precios = res.ToList();
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        try
        {
            var maxAllowedFiles = 1;
            var format = new string[] { "image/png", "image/jpeg", "image/jpg", "image/bmp" };

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                if (!format.Contains(file.ContentType))
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Se ha producido un error",
                        Detail = "El formato de archivo no es válido",
                        Duration = 4000
                    });
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await file.OpenReadStream(10 * 1024 * 1024).CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    Model.ArrayFileData = fileBytes;
                    Model.Imagen = file.Name;
                    fileData = $"data:{file.ContentType};base64,{Convert.ToBase64String(fileBytes)}";
                }
            }
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }


}
