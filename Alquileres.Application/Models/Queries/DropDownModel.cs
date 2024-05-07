namespace Alquileres.Application.Models.Queries;

public class DropDownModel
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? CssStyle { get; set; }

    public bool Overflow { get; set; } = true;

    public bool Disabled { get; set; }

    public DropDownModel()
    {

    }
    public DropDownModel(PlataformaListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = dto.Nombre;
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;

    }

    public DropDownModel(GeneroListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = dto.Nombre;
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;
    }

    public DropDownModel(LineaGeneroDTO dto)
    {
        this.Id = dto.IdGenero;


    }
    public DropDownModel(FormaPagoListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = dto.Nombre;
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;
    }

    public DropDownModel(ClienteListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = $"{dto.Dni} - {dto.Code} - {dto.Nombre} {dto.Apellidos}";
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;
    }

    public DropDownModel(VideoJuegoListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = dto.Nombre;
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;
    }

    public DropDownModel(PrecioVideoJuegoListDTO dto)
    {
        this.Id = dto.Id;
        this.Nombre = $" {dto.NombrePlataforma} - {dto.Precio}€ / Día ";
        this.CssStyle = dto.Activado ? "" : "color: red !important;";
        this.Disabled = !dto.Activado;
    }
}
