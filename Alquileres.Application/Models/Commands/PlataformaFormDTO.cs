using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Commands;

public record class PlataformaFormDTO
{
    public int Id { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Plataforma", Name = "Plataforma")]
    public string? Nombre { get; set; } = null!;

    [FormFieldDisplay(typeof(string), Label = "Version", Name = "Version")]
    public string? Version { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Company", Name = "Company")]
    public string? Company { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Activado", Name = "Activado")]
    public bool Activado { get; set; }

    public PlataformaFormDTO()
    {

    }

    public PlataformaFormDTO(int id, string plataforma, string version, string company, bool activado)
    {
        Id = id;
        Nombre = plataforma;
        Version = version;
        Company = company;
        Activado = activado;
    }

    public PlataformaFormDTO(PlataformaFormDTO p)
    {
        Id = p.Id;
        Nombre = p.Nombre;
        Version = p.Version;
        Company = p.Company;
        Activado = p.Activado;

    }
}
