using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Application.Queries.Alquiler;
using Alquileres.Application.Queries.Cliente;
using Alquileres.Components.Shared;
using Alquileres.Helpers;
using Microsoft.AspNetCore.Components;

namespace Alquileres.Components.Pages.Cliente;

public partial class DetalleCliente : BaseComponent
{
    [Parameter]
    public int ClienteId { get; set; }

    public ClienteFormDTO Cliente { get; set; } = new();

    public IList<AlquilerListDTO> listasAlquiler { get; set; } = new List<AlquilerListDTO>();

    string fileData = "img/id-card.png";

    protected override async Task OnInitializedAsync()
    {

        try
        {
            Cliente = await Mediator.Send(new GetClienteByIdQuery(ClienteId));

            listasAlquiler = await Mediator.Send(new GetAlquilerByIdClienteQuery(ClienteId));

            if (Cliente.Documento != null)
            {
                fileData = $"data:{Cliente.Documento.Split(".")[1]};base64,{Cliente.Fichero}";
            }
        }
        catch (Exception ex)
        {

            await SweetAlertHelper.ShowThrowErrorAlertAsync(Swal, ex);
        }

    }

    async Task EditarCliente()
    {
        NavigationManager.NavigateTo($"cliente/edit/{ClienteId}", true);
    }

}
