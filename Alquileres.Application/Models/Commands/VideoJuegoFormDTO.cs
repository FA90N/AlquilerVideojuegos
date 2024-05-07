namespace Alquileres.Application.Models.Commands;

public record class VideoJuegoFormDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? FechaLanzamiento { get; set; }

    public decimal? Volumen { get; set; }

    public string? Descripcion { get; set; }

    public string? Desarrollador { get; set; }

    public string? Pegi { get; set; }

    public bool Activado { get; set; }

    public string? Imagen { get; set; }

    public string Fichero { get; set; }

    public byte[] ArrayFileData { get; set; }


    public VideoJuegoFormDTO()
    {

    }

    public VideoJuegoFormDTO(int id, string nombre, DateTime? fechaLanzamiento, int? volumen, string? descripcion, string? desarrollador, String pegi, bool activado, string fichero, byte[] array, string imagen)
    {
        Id = id;
        Nombre = nombre;
        FechaLanzamiento = fechaLanzamiento;
        Volumen = volumen;
        Descripcion = descripcion;
        Desarrollador = desarrollador;
        Pegi = pegi;
        Activado = activado;
        Fichero = fichero;
        ArrayFileData = array;
        Imagen = imagen;
    }

    public VideoJuegoFormDTO(VideoJuegoFormDTO v)
    {

        Id = v.Id;
        Nombre = v.Nombre;
        FechaLanzamiento = v.FechaLanzamiento;
        Volumen = v.Volumen;
        Descripcion = v.Descripcion;
        Desarrollador = v.Desarrollador;
        Pegi = v.Pegi;
        Imagen = v.Imagen;
        Activado = v.Activado;
        Fichero = v.Fichero;
        ArrayFileData = v.ArrayFileData;
    }
}
