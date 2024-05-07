using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public record class LineaAlquilerDTO
{
    public int Id { get; set; }
    public int IdAlquiler { get; set; }
    public int IdPrecioVideojuego { get; set; }

    [GridModelDisplay(Name = "Juego")]
    public string Juego { get; set; } = null!;

    [GridModelDisplay(Name = "Plataforma")]
    public string Plataforma { get; set; } = null!;

    [GridModelDisplay(Name = "Unidades")]
    public int Cantidad { get; set; }

    [GridModelDisplay(Name = "Precio", FormatString = "{0:C}", Width = "130px")]
    public decimal Precio { get; set; }

    [GridModelDisplay(Name = "Total", FormatString = "{0:C}", Width = "140px", HasFooter = true, FooterText = "Importe Total por Día")]
    public decimal Total { get; set; }
}
