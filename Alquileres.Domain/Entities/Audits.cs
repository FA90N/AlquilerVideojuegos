using System.ComponentModel.DataAnnotations;

namespace Alquileres.Domain.Entities
{
    public class Audits
    {
        [Key]
        public long Id { get; set; }

        public string? Type { get; set; }

        public string? TableName { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public string? AffectedColumns { get; set; }

        public string? PrimaryKey { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}