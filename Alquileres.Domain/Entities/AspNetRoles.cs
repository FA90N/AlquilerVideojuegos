using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alquileres.Domain.Entities;

public class AspNetRoles
{
    [Key]
    public string Id { get; set; } = null!;

    [StringLength(256)]
    public string? Name { get; set; }

    [StringLength(256)]
    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; } = new List<AspNetRoleClaims>();

    [ForeignKey("RoleId")]
    [InverseProperty("Role")]
    public virtual ICollection<AspNetUsers> User { get; } = new List<AspNetUsers>();
}