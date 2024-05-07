
namespace Alquileres.Application.Interfaces.Application;

public interface IExportServices
{
    string BuildJson(string jsonString, Dictionary<string, string> replacements);
    byte[] ToCSV(string jsonString, string? fileName = null);
    byte[] ToExcel(string jsonString, string? fileName = null);
    byte[] ToPdf(string jsonString, string? fileName = null);
}