using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alquileres.Domain.Entities;

[PrimaryKey("LoginProvider", "ProviderKey")]
public class AspNetUserLogins
{
    [Key]
    [StringLength(128)]
    public string LoginProvider { get; set; } = null!;

    [Key]
    [StringLength(128)]
    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AspNetUserLogins")]
    public virtual AspNetUsers User { get; set; } = null!;
}