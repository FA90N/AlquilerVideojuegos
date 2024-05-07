using Alquileres.Application.Interfaces.Application;
using SelectPdf;
using PdfDocument = SelectPdf.PdfDocument;

namespace Alquileres.Application.Services;

public class HtmlToPdfService : IHtmlToPdfService
{
    public enum PdfPageSizeType
    {
        A4,
        A5
    }

    public byte[] ConvertHtmlToPdf(string htmlString, PdfPageSizeType pageSize = PdfPageSizeType.A4)
    {
        // Inicializar el conversor de HTML a PDF
        HtmlToPdf converter = new HtmlToPdf();

        // Por ejemplo: configurar tamaño de página, márgenes, etc.
        converter.Options.PdfPageSize = pageSize == PdfPageSizeType.A4 ? PdfPageSize.A4 : PdfPageSize.A5;
        converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
        converter.Options.MarginBottom = 25;
        converter.Options.MarginLeft = 25;
        converter.Options.MarginRight = 25;
        converter.Options.MarginTop = 25;

        // Convertir HTML a PDF
        PdfDocument doc = converter.ConvertHtmlString(htmlString);

        // Crear un MemoryStream para guardar el PDF
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Guardar el documento PDF en el MemoryStream
            doc.Save(memoryStream);

            // Cerrar el documento PDF
            doc.Close();

            // Retornar el contenido del MemoryStream como un array de bytes
            return memoryStream.ToArray();
        }
    }
}
