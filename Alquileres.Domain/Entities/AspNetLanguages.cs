using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alquileres.Domain.Entities;

public class AspNetLanguages
{
    [Key]
    [StringLength(5)]
    public string Id { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("AspNetLanguages")]
    public virtual ICollection<AspNetUsers> AspNetUsers { get; } = new List<AspNetUsers>();
}
