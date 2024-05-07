using Alquileres.Application.Enums;
using Alquileres.Application.Extensions;

namespace Alquileres.Application.Models.Queries;

public sealed record class AlquilerListDTO
{
    public int Id { get; set; }

    [GridModelDisplay(Name = "Número Alquiler", Width = "150px")]
    public string Codigo { get; set; }

    [GridModelDisplay(Name = "Fecha", Width = "150px", FormatString = "{0:dd/MM/yyyy}", Sort = SortOrderEnum.Descending)]
    public DateTime Fecha { get; set; }

    [GridModelDisplay(Name = "Cliente")]
    public string NombreCliente { get; set; }

    [GridModelDisplay(Name = "DNI")]
    public string DNI { get; set; }

    [GridModelDisplay(Name = "Forma de pago")]
    public string FormaPago { get; set; }

    [GridModelDisplay(Name = "Total", FormatString = "{0:C}", Width = "130px")]
    public decimal Total { get; set; }
}
