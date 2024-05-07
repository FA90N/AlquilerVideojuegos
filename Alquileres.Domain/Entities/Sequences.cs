using Alquileres.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Alquileres.Domain.Entities;

public class Sequences : BaseEntity
{
    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    public int LastNumber { get; set; }

    [StringLength(5)]
    public string LastNumberFormat { get; set; } = null!;

    public bool ResetYear { get; set; }

    public int? Year { get; set; }
}
