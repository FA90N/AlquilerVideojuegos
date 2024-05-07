using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Commands;

public record class GeneroFormDTO
{
    public int Id { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Genero", Name = "Genero")]
    public string Nombre { get; set; } = null!;


    [FormFieldDisplay(typeof(string), Label = "En uso", Name = "Activado")]
    public bool Activado { get; set; }

    public GeneroFormDTO()
    {

    }

    public GeneroFormDTO(int id, string genero, bool activado)
    {
        Id = id;
        Nombre = genero;
        Activado = activado;
    }

    public GeneroFormDTO(GeneroFormDTO o)
    {
        Id = o.Id;
        Nombre = o.Nombre;
        Activado = o.Activado;
    }
}
