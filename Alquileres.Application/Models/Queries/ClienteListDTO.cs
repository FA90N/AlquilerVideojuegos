using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class ClienteListDTO
{


    public int Id { get; set; }

    public string Code { get; set; }

    public DateTime? FechaAlta { get; set; }

    [GridModelDisplay(Name = "Nombre")]
    public string Nombre { get; set; } = null!;

    [GridModelDisplay(Name = "Apellidos")]
    public string Apellidos { get; set; } = null!;

    [GridModelDisplay(Name = "Dni")]
    public string Dni { get; set; } = null!;

    public DateTime? FechaNacimiento { get; set; }
    public string? Comentario { get; set; }

    [GridModelDisplay(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [GridModelDisplay(Name = "Correo electrónico")]
    public string? Email { get; set; }

    [GridModelDisplay(Name = "Activado")]
    public bool Activado { get; set; }
}