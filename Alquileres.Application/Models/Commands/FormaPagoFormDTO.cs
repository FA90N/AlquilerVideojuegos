using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Commands;

public record class FormaPagoFormDTO
{

    public int Id { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Forma de pago", Name = "Nombre")]
    public string Nombre { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Descripcion ", Name = "Descripcion")]
    public string Descripcion { get; set; }

    [FormFieldDisplay(typeof(string), Label = "Activado", Name = "Activado")]
    public bool Activado { get; set; }

    public FormaPagoFormDTO()
    {

    }


    public FormaPagoFormDTO(FormaPagoFormDTO o)
    {
        Id = o.Id;
        Nombre = o.Nombre;
        Descripcion = o.Descripcion;
        Activado = o.Activado;
    }

    public FormaPagoFormDTO(int id, string nombre, string descripcion, bool activado)
    {
        Id = id;
        Nombre = nombre;
        Descripcion = descripcion;
        Activado = activado;
    }
}
