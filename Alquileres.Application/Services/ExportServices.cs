using Alquileres.Application.Interfaces.Application;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Table = iText.Layout.Element.Table;

namespace Alquileres.Application.Services;

public class ExportServices : IExportServices
{
    public byte[] ToCSV(string jsonString, string? fileName = null)
    {
        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonString)!;

        StringBuilder sb = new StringBuilder();

        // Escribir los nombres de las columnas
        IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

        sb.AppendLine(string.Join(",", columnNames));

        // Escribir las filas
        foreach (DataRow row in dataTable.Rows)
        {
            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
            sb.AppendLine(string.Join(",", fields));
        }

        // Crear un MemoryStream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Escribir en el MemoryStream usando un StreamWriter
            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write(sb.ToString());
                writer.Flush();
                memoryStream.Position = 0;

                // Convertir el MemoryStream en un array de bytes
                byte[] bytes = memoryStream.ToArray();

                return bytes;
            }
        }
    }

    public byte[] ToExcel(string jsonString, string? fileName = null)
    {
        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonString)!;

        var stream = new MemoryStream();

        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();

            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            GenerateWorkbookStylesPartContent(workbookStylesPart);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
            sheets.Append(sheet);

            workbookPart.Workbook.Save();

            var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

            var headerRow = new Row();

            foreach (DataColumn column in dataTable.Columns)
            {
                Cell cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(column.ColumnName) };
                headerRow.AppendChild(cell);
            }

            sheetData.AppendChild(headerRow);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                Row newRow = new Row();
                foreach (var cellValue in dataRow.ItemArray)
                {
                    Cell cell = new Cell() { DataType = CellValues.String, CellValue = new CellValue(TransformTrueFalseToYesNo(cellValue)) };
                    newRow.AppendChild(cell);
                }
                sheetData.AppendChild(newRow);
            }

            workbookPart.Workbook.Save();
        }

        if (stream?.Length > 0)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }

        return stream!.ToArray();
    }

    public byte[] ToPdf(string jsonString, string? fileName = null)
    {
        DataTable dataTable = JsonConvert.DeserializeObject<DataTable>(jsonString)!;

        // Crear un MemoryStream para almacenar el PDF
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Crear un objeto PdfWriter asociado con el MemoryStream
            PdfWriter writer = new PdfWriter(memoryStream);

            writer.SetCloseStream(false);

            // Crear un objeto PdfDocument
            PdfDocument pdf = new PdfDocument(writer);

            // Crear un documento
            Document document = new Document(pdf, PageSize.A4.Rotate());

            // Crear una tabla con la cantidad de columnas del DataTable
            Table table = new Table(dataTable.Columns.Count);

            // Agregar los encabezados de la tabla
            foreach (DataColumn column in dataTable.Columns)
            {
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new Paragraph(column.ColumnName)));
            }

            // Agregar las filas de la tabla
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (var cellValue in row.ItemArray)
                {
                    table.AddCell(new iText.Layout.Element.Cell().Add(new Paragraph(TransformTrueFalseToYesNo(cellValue))));
                }
            }

            // Agregar la tabla al documento
            document.Add(table);

            // Cerrar el documento
            document.Close();

            // Convertir el MemoryStream en un array de bytes
            return memoryStream.ToArray();
        }
    }

    public string BuildJson(string jsonString, Dictionary<string, string> replacements)
    {
        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            // Crear un objeto JsonElement a partir del documento
            JsonElement root = doc.RootElement;

            // Utilizar un Utf8JsonWriter para escribir el JSON modificado
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        writer.WriteStartArray();

                        foreach (JsonElement element in doc.RootElement.EnumerateArray())
                        {
                            if (element.ValueKind == JsonValueKind.Object)
                            {
                                writer.WriteStartObject();

                                foreach (JsonProperty prop in element.EnumerateObject())
                                {
                                    // Si la propiedad actual tiene una traducción, escribirla con la clave traducida
                                    if (replacements.TryGetValue(prop.Name, out string? propertyNameTitle))
                                    {
                                        writer.WritePropertyName(propertyNameTitle);
                                        prop.Value.WriteTo(writer);
                                    }
                                }

                                writer.WriteEndObject();
                            }
                        }

                        writer.WriteEndArray();
                    }
                }

                // Convertir el MemoryStream a una cadena
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }

    private string? TransformTrueFalseToYesNo(object cellValue)
    {
        var cell = cellValue is not null ? cellValue.ToString() : "";

        if (cellValue is not null && cellValue.GetType() == typeof(bool))
        {
            cell = cellValue!.ToString()!.Replace("True", "Sí").Replace("False", "No");
        }

        return cell;
    }

    private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
    {
        Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2 xr" } };
        stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
        stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
        stylesheet1.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
        stylesheet1.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

        Fonts fonts1 = new Fonts() { Count = (UInt32Value)1U, KnownFonts = true };

        Font font1 = new Font();
        FontSize fontSize1 = new FontSize() { Val = 11D };
        Color color1 = new Color() { Theme = (UInt32Value)1U };
        FontName fontName1 = new FontName() { Val = "Calibri" };
        FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
        FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

        font1.Append(fontSize1);
        font1.Append(color1);
        font1.Append(fontName1);
        font1.Append(fontFamilyNumbering1);
        font1.Append(fontScheme1);

        fonts1.Append(font1);

        Fills fills1 = new Fills() { Count = (UInt32Value)2U };

        Fill fill1 = new Fill();
        PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

        fill1.Append(patternFill1);

        Fill fill2 = new Fill();
        PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

        fill2.Append(patternFill2);

        fills1.Append(fill1);
        fills1.Append(fill2);

        Borders borders1 = new Borders() { Count = (UInt32Value)1U };

        Border border1 = new Border();
        LeftBorder leftBorder1 = new LeftBorder();
        RightBorder rightBorder1 = new RightBorder();
        TopBorder topBorder1 = new TopBorder();
        BottomBorder bottomBorder1 = new BottomBorder();
        DiagonalBorder diagonalBorder1 = new DiagonalBorder();

        border1.Append(leftBorder1);
        border1.Append(rightBorder1);
        border1.Append(topBorder1);
        border1.Append(bottomBorder1);
        border1.Append(diagonalBorder1);

        borders1.Append(border1);

        CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
        CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

        cellStyleFormats1.Append(cellFormat1);

        CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)2U };
        CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
        CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)14U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyNumberFormat = true };

        cellFormats1.Append(cellFormat2);
        cellFormats1.Append(cellFormat3);

        CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
        CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

        cellStyles1.Append(cellStyle1);
        DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
        TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

        StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

        StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
        stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");

        StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
        stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

        //OpenXmlUnknownElement openXmlUnknownElement4 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

        //stylesheetExtension2.Append(openXmlUnknownElement4);

        stylesheetExtensionList1.Append(stylesheetExtension1);
        stylesheetExtensionList1.Append(stylesheetExtension2);

        stylesheet1.Append(fonts1);
        stylesheet1.Append(fills1);
        stylesheet1.Append(borders1);
        stylesheet1.Append(cellStyleFormats1);
        stylesheet1.Append(cellFormats1);
        stylesheet1.Append(cellStyles1);
        stylesheet1.Append(differentialFormats1);
        stylesheet1.Append(tableStyles1);
        stylesheet1.Append(stylesheetExtensionList1);

        workbookStylesPart1.Stylesheet = stylesheet1;
    }
}
