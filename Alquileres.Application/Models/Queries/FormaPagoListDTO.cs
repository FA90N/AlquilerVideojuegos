using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class FormaPagoListDTO
{

    public int Id { get; set; }

    [GridModelDisplay(Name = "Método de  pago")]
    public string Nombre { get; set; }

    [GridModelDisplay(Name = "Descripción")]
    public string Descripcion { get; set; }

    [GridModelDisplay(Name = "En uso")]
    public bool Activado { get; set; }

}
