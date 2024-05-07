using System.Text.Json.Serialization;

namespace Alquileres.Application.Models
{
    public sealed class UserSignInDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}