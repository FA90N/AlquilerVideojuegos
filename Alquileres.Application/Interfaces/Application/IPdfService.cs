namespace Alquileres.Application.Interfaces.Application;

public interface IPdfService
{
    byte[] MergePDFs(List<byte[]> pdfByteArraysList);
}