using System.Text.Json.Serialization;

namespace Alquileres.Application.Models
{
    public sealed class TokenResultDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = null!;
    }
}
