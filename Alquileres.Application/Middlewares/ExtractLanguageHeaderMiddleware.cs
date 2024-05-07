using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Alquileres.Application.Middlewares
{
    public class ExtractLanguageHeaderMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Headers.TryGetValue("Content-Language", out StringValues headerValue))
            {
                var cultureInfo = CultureInfo.GetCultureInfo(headerValue.ToString());

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            await next(context);
        }
    }
}
