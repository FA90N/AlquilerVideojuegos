using Alquileres.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;

namespace Alquileres.Application.Configuration.Middlewares
{
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = StatusCodes.Status500InternalServerError;
                var result = string.Empty;

                switch (ex)
                {
                    case NotFoundException notFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        break;

                    case ValidationException validationException:
                        statusCode = StatusCodes.Status400BadRequest;
                        var validationJson = JsonSerializer.Serialize(validationException.Errors);
                        result = JsonSerializer.Serialize(new CodeErrorException(statusCode, ex.Message, validationJson));
                        break;

                    case BadRequestException badRequestException:
                        statusCode = StatusCodes.Status400BadRequest;
                        break;

                    default:
                        break;
                }

                if (string.IsNullOrEmpty(result))
                    result = JsonSerializer.Serialize(new CodeErrorException(statusCode, ex.Message, ex.StackTrace));

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsync(result);
            }
        }
    }
}