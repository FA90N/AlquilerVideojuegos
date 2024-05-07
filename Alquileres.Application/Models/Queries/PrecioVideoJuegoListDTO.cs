using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class PrecioVideoJuegoListDTO
{
    public int Id { get; set; }

    [GridModelDisplay(Name = "Plataforma")]
    public string NombrePlataforma { get; set; }

    [GridModelDisplay(Name = "Precio por dia", FormatString = "{0:C}", Width = "130px")]
    public decimal? Precio { get; set; }

    [GridModelDisplay(Name = "Activado")]
    public bool Activado { get; set; }
}
