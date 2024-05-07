using System.Text.Json.Serialization;

namespace Alquileres.Application.Models
{
    public sealed class SendEmailDto
    {
        [JsonPropertyName("from")]
        public string? From { get; set; }

        [JsonPropertyName("to")]
        public List<string>? To { get; set; }

        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("template")]
        public string? Template { get; set; }

        [JsonPropertyName("dynamicTemplateData")]
        public object? DynamicTemplateData { get; set; }

        [JsonPropertyName("attachments")]
        public List<KeyValuePair<string, string>>? Attachments { get; set; }
    }
}