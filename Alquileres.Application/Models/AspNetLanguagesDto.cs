using System.Text.Json.Serialization;

namespace Alquileres.Application.Models;

public sealed class AspNetLanguagesDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}
