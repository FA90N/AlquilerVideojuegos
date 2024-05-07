using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class PlataformaListDTO
{
    public int Id { get; set; }

    [GridModelDisplay(Name = "Plataforma")]
    public string Nombre { get; set; } = null!;

    [GridModelDisplay(Name = "Versión")]
    public string? Version { get; set; }

    [GridModelDisplay(Name = "Compañia")]
    public string? Company { get; set; }

    [GridModelDisplay(Name = "En uso")]
    public bool Activado { get; set; }

}
