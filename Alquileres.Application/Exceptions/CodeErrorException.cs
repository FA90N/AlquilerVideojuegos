using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Alquileres.Application.Exceptions
{
    public sealed class CodeErrorException
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("details")]
        public string? Details { get; set; }

        public CodeErrorException(int statusCode, string? message = null, string? details = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
            Details = details;
        }

        private string GetDefaultMessageStatusCode(int statusCode) => statusCode switch
        {
            StatusCodes.Status400BadRequest => "La petición enviada contiene errores. XD",
            StatusCodes.Status401Unauthorized => "No tienes permiso para acceder al recurso solicitado, comprueba el token... ¡Puerco!",
            StatusCodes.Status404NotFound => "No se encontro el recurso solicitado. ¡Revisa la URL cenutrio!",
            StatusCodes.Status500InternalServerError => "Se ha producido un error interno en el servidor. ¡Vaya paquetes!",
            _ => string.Empty
        };
    }
}