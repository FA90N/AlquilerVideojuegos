namespace Alquileres.Application.Models.Commands;


public record class ClienteFormDTO
{
    public int Id { get; set; }


    public string Code { get; set; } = null!;


    public DateTime FechaAlta { get; set; }


    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public DateTime? FechaNacimiento { get; set; } = null!;

    public bool Activado { get; set; }


    public string Comentario { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Documento { get; set; }

    public string Email { get; set; }

    public string Fichero { get; set; }

    public byte[] ArrayFileData { get; set; }


    public ClienteFormDTO()
    {

    }

    public ClienteFormDTO(int id, string code, DateTime fechaAlta, string nombre, string apellidos, string dni, DateTime fechaNacimiento, bool active, string comentario, string telefono, string email, string fichero, byte[] array, string documento)
    {
        Id = id;
        Code = code;
        FechaAlta = fechaAlta;
        Nombre = nombre;
        Apellidos = apellidos;
        Dni = dni;
        FechaNacimiento = fechaNacimiento;
        Activado = active;
        Comentario = comentario;
        Telefono = telefono;
        Email = email;
        Fichero = fichero;
        Documento = documento;
        ArrayFileData = array;
    }

    public ClienteFormDTO(ClienteFormDTO o)
    {
        Id = o.Id;
        Code = o.Code;
        FechaAlta = o.FechaAlta;
        Nombre = o.Nombre;
        Apellidos = o.Apellidos;
        Dni = o.Dni;
        FechaNacimiento = o.FechaNacimiento;
        Activado = o.Activado;
        Comentario = o.Comentario;
        Telefono = o.Telefono;
        Email = o.Email;
        Fichero = o.Fichero;
        Documento = o.Documento;
        ArrayFileData = o.ArrayFileData;
    }
}
