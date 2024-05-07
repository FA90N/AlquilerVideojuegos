namespace Alquileres.Application.Models.Commands;

public record class LineaAlquilerFormDTO
{
    public int Id { get; set; }
    public int IdAlquiler { get; set; }
    public int IdPrecioVideojuego { get; set; }
    public string Comentarios { get; set; }
    public int Cantidad { get; set; }

    public LineaAlquilerFormDTO()
    {


    }

    public LineaAlquilerFormDTO(int id, int idAlquiler, int idPrecioVideojuego, string comentarios, int cantidad)
    {
        Id = id;
        IdAlquiler = idAlquiler;
        IdPrecioVideojuego = idPrecioVideojuego;
        Comentarios = comentarios;
        Cantidad = cantidad;
    }

    public LineaAlquilerFormDTO(LineaAlquilerFormDTO l)
    {
        Id = l.Id;
        IdAlquiler = l.IdAlquiler;
        IdPrecioVideojuego = l.IdPrecioVideojuego;
        Comentarios = l.Comentarios;
        Cantidad = l.Cantidad;
    }
}
