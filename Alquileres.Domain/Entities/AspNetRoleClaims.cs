using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alquileres.Domain.Entities;

public class AspNetRoleClaims
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string RoleId { get; set; } = null!;

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("AspNetRoleClaims")]
    public virtual AspNetRoles Role { get; set; } = null!;
}