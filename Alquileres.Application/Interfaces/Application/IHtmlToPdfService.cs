using Alquileres.Application.Services;

namespace Alquileres.Application.Interfaces.Application;

public interface IHtmlToPdfService
{
    byte[] ConvertHtmlToPdf(string htmlString, HtmlToPdfService.PdfPageSizeType pageSize = HtmlToPdfService.PdfPageSizeType.A4);
}