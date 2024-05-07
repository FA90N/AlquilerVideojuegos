using System.Text.Json.Serialization;

namespace Alquileres.Application.Models
{
    public class AspNetUsersCompaniesDto
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; } = null!;
    }
}
