using Alquileres.Application.Commands.Cliente;
using Alquileres.Application.Exceptions;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Queries.Cliente;
using Alquileres.Application.Queries.Sequences;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Alquileres.Components.Pages.Cliente;

public partial class ClienteEdit : BaseComponent
{
    [Parameter]
    public int ClienteId { get; set; }

    public ClienteFormDTO Model { get; set; } = new();

    public bool IsNew { get; set; }

    string fileData;


    protected override async Task OnInitializedAsync()
    {
        try
        {
            await base.OnInitializedAsync();

            if (ClienteId != 0)
            {
                Model = await Mediator.Send(new GetClienteByIdQuery(ClienteId));
            }
            else
            {
                Model = new()
                {
                    Code = (await Mediator.Send(new GetSequencesByEntityNameQuery(SequencesEntityName.Cliente))).LastNumberFormat,
                    Activado = true,
                    FechaAlta = DateTime.Now.Date
                };

            }
            if (Model.Fichero != null)
            {
                fileData = $"data:{Model.Documento.Split(".")[1]};base64,{Model.Fichero}";
            }
            IsNew = ClienteId == 0;
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
                    Model.Documento = file.Name;
                    fileData = $"data:{file.ContentType};base64,{Convert.ToBase64String(fileBytes)}";
                }
            }
        }
        catch (Exception ex)
        {
            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }
    }

    //Comprobar si existe el dni
    bool ComprobarDNI(string dni)
    {

        var res = Mediator.Send(new GetClienteByDniQuery(dni));
        if (res.Result != null)
        {
            if (Model.Id != res.Result.Id)
            {
                return true;
            }
        }

        return false;

    }

    async Task OnSubmit(ClienteFormDTO cliente)
    {
        try
        {


            if (string.IsNullOrEmpty(Model.Telefono) && string.IsNullOrEmpty(Model.Email))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Se ha producido un error al validar el formulario",
                    Detail = "Debe rellenar mínino telefono o correo electrónico",
                    Duration = 4000
                });
            }
            else
            {
                if (!IsNew)
                {
                    await Mediator.Send(new UpdateClienteCommand(cliente));
                }
                else
                {
                    ClienteId = await Mediator.Send(new CreateClienteCommand(cliente));
                }



                NavigationManager.NavigateTo("/cliente", true, true);

                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Éxito!",
                    Detail = "Los datos del cliente han sido guardados satisfactoriamente",
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
        NavigationManager.NavigateTo("/cliente", true, true);
    }


}
