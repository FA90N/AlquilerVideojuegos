namespace Alquileres.Application.Models.Commands;

public record class AlquilerFormDTO
{
    public int Id { get; set; }

    public string Code { get; set; }

    public int IdCliente { get; set; }

    public DateTime Fecha { get; set; }

    public DateTime FechaFin { get; set; }

    public int Dias { get; set; }

    public int IdFormaPago { get; set; }

    public AlquilerFormDTO()
    {

    }

    public AlquilerFormDTO(int id, string code, int idCliente, DateTime fecha, DateTime fechaFin, int dias, int idFormaPago)
    {
        Id = id;
        Code = code;
        IdCliente = idCliente;
        Fecha = fecha;
        FechaFin = fechaFin;
        Dias = dias;
        IdFormaPago = idFormaPago;
    }

    public AlquilerFormDTO(AlquilerFormDTO o)
    {
        Id = o.Id;
        Code = o.Code;
        IdCliente = o.IdCliente;
        Fecha = o.Fecha;
        FechaFin = o.FechaFin;
        Dias = o.Dias;
        IdFormaPago = o.IdFormaPago;
    }
}
