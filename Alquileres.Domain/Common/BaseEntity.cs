using System.ComponentModel.DataAnnotations;

namespace Alquileres.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}