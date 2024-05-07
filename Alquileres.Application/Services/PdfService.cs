using Alquileres.Application.Interfaces.Application;
using iText.Kernel.Pdf;

namespace Alquileres.Application.Services;

public class PdfService : IPdfService
{
    public byte[] MergePDFs(List<byte[]> pdfByteArraysList)
    {
        using (MemoryStream mergedPdfStream = new MemoryStream())
        {
            // Abrir el documento PDF resultante
            using (PdfWriter writer = new PdfWriter(mergedPdfStream))
            {
                using (PdfDocument pdfDocument = new PdfDocument(writer))
                {
                    // Iterar sobre la lista de ByteArray y agregar las páginas al documento resultante
                    foreach (var pdfByteArray in pdfByteArraysList)
                    {
                        using (MemoryStream pdfMemoryStream = new MemoryStream(pdfByteArray))
                        {
                            using (PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfMemoryStream)))
                            {
                                int numPages = pdfDoc.GetNumberOfPages();
                                for (int pageNum = 1; pageNum <= numPages; pageNum++)
                                {
                                    pdfDocument.AddPage(pdfDoc.GetPage(pageNum).CopyTo(pdfDocument));
                                }
                            }
                        }
                    }
                }
            }

            return mergedPdfStream.ToArray();
        }
    }
}
