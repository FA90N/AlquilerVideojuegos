using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Extensions;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Alquileres.Application.Queries.Alquiler;

public record class GetAlquilerReportQuery(int AlquilerId) : IQuery<Byte[]>;
internal class GetAlquilerReportQueryHandler : IQueryHandler<GetAlquilerReportQuery, byte[]>
{
    private readonly IRepositoryBase<Domain.Entities.Alquiler> _repository;
    private readonly IHtmlToPdfService _htmlToPdfService;
    private readonly IPdfService _pdfService;


    public GetAlquilerReportQueryHandler(IRepositoryBase<Domain.Entities.Alquiler> repository, IHtmlToPdfService htmlToPdfService, IPdfService pdfService)
    {
        _repository = repository;
        _htmlToPdfService = htmlToPdfService;
        _pdfService = pdfService;

    }

    public async Task<byte[]> Handle(GetAlquilerReportQuery request, CancellationToken cancellationToken)
    {
        var ci = new CultureInfo("es-Es");

        var data = await _repository.GetQueryable()
            .Include(x => x.ClienteNavigation)
            .Include(x => x.FormaPagoNavigation)
            .Include(x => x.LineasAlquileres)
            .ThenInclude(xi => xi.PrecioPlataformasNavigation)
            .ThenInclude(xi => xi.VideoJuegoNavigation)
            .Include(x => x.LineasAlquileres)
            .ThenInclude(xi => xi.PrecioPlataformasNavigation)
            .ThenInclude(xi => xi.PlataformaNavigation)

            .SingleAsync(s => s.Id == request.AlquilerId, cancellationToken: cancellationToken);


        var TotalFactura = data.LineasAlquileres.Sum(x => (x.Cantidad * x.PrecioPlataformasNavigation.Precio) * x.AlquilerNavigation.Dias);
        var lineasPedido = "";
        var templateLine = @"<tr>
                        <td class=""text-left"">#lineasPedido_Juego#</td>
                        <td class=""text-left"">#lineasPedido_Plataforma#</td>
                        <td class=""text-center"">#lineasPedido_Descripcion#</td>
                        <td class=""text-center"">#lineasPedido_Cantidad#</td>
                        <td class=""text-center"">#lineasPedido_Precio#</td>
                        <td class=""text-right"">#lineasPedido_Total#</td>
                    </tr>";

        foreach (var item in data.LineasAlquileres)
        {
            var comentarios = "";
            if (item.Comentarios.IsNotNullOrEmpty())
            {
                if (item.Comentarios.Length < 50)
                {
                    comentarios = item.Comentarios.ToString();
                }
                else
                {
                    comentarios = item.Comentarios.ToString()[..50] + " ...";
                }
            }

            item.Comentarios.IsNotNullOrEmpty();

            lineasPedido += templateLine.Replace("#lineasPedido_Juego#", item.PrecioPlataformasNavigation.VideoJuegoNavigation.Nombre)
                                        .Replace("#lineasPedido_Plataforma#", item.PrecioPlataformasNavigation.PlataformaNavigation.Nombre)
                                        .Replace("#lineasPedido_Descripcion#", comentarios)
                                        .Replace("#lineasPedido_Cantidad#", item.Cantidad.ToString())
                                        .Replace("#lineasPedido_Precio#", item.PrecioPlataformasNavigation.Precio.ToString("C", ci) + " x " + item.AlquilerNavigation.Dias.ToString() + " días")
                                        .Replace("#lineasPedido_Total#", ((item.PrecioPlataformasNavigation.Precio * item.Cantidad) * item.AlquilerNavigation.Dias).ToString("C", ci));
        }
        var template = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Alquiler.html"));

        template = template.Replace("#Cliente_nombre#", data.ClienteNavigation.Nombre + " " + data.ClienteNavigation.Apellidos)
                           .Replace("#Cliente_email#", data.ClienteNavigation.Email)
                           .Replace("#Cliente_telefono#", data.ClienteNavigation.Telefono)
                           .Replace("#Alquiler_fecha#", data.Fecha.ToString("dd/MM/yyyy"))
                           .Replace("#Alquiler_devolucion#", data.FechaFin.ToString("dd/MM/yyyy"))
                           .Replace("#Total_dias#", data.Dias.ToString())
                           .Replace("#Cliente_DNI#", data.ClienteNavigation.Dni)
                           .Replace("#Cliente_Code#", data.ClienteNavigation.Code)
                           .Replace("#Total#", TotalFactura.ToString("C", ci))
                           .Replace("#FormaPago#", data.FormaPagoNavigation.Nombre)
                           .Replace("#Alquileres_Linea#", lineasPedido);


        var AlquilerPDF = _htmlToPdfService.ConvertHtmlToPdf(template, HtmlToPdfService.PdfPageSizeType.A4);
        return _pdfService.MergePDFs([AlquilerPDF]);
    }
}
