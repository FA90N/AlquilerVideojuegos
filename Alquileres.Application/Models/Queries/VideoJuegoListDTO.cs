using Alquileres.Application.Enums;
using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class VideoJuegoListDTO
{
    public int Id { get; set; }

    [GridModelDisplay(Name = "Juego")]
    public string Nombre { get; set; } = null!;

    [GridModelDisplay(Name = "Fecha de lanzamiento", FormatString = "{0:dd/MM/yyyy}", Sort = SortOrderEnum.Descending)]
    public DateTime? FechaLanzamiento { get; set; }

    [GridModelDisplay(Name = "Puntuación")]
    public string? Pegi { get; set; }

    [GridModelDisplay(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [GridModelDisplay(Name = "Desarrollador")]
    public string? Desarrollador { get; set; }

    [GridModelDisplay(Name = "Activado")]
    public bool Activado { get; set; }

}
