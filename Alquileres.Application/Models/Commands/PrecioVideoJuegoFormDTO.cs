using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Commands;

public record class PrecioVideoJuegoFormDTO
{
    public int Id { get; set; }

    public int IdVideoJuego { get; set; }

    public string Plataforma { get; set; }

    public int IdPlataforma { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Precio", Name = "Precio")]
    public decimal? Precio { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Activado", Name = "Activado")]
    public bool? Activado { get; set; }

}
