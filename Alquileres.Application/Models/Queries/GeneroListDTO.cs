using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class GeneroListDTO
{
    [GridModelDisplay(Name = "Codigo")]
    public int Id { get; set; }

    [GridModelDisplay(Name = "Género")]
    public string Nombre { get; set; } = null!;

    [GridModelDisplay(Name = "En uso")]
    public bool Activado { get; set; }

}
