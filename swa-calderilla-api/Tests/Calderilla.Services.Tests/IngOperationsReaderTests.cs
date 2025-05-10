using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Calderilla.Services.ProcessIngService;

namespace Calderilla.Services.Tests
{
    public class IngOperationsReaderTests
    {
        private const int TestMonth = 12;
        private const int TestYear = 2023;

        [Fact]
        public void GetIngOperations_ForSpecificMonth_ShouldReturnFilteredOperations()
        {
            // Arrange  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            CreateHeaderRows(sheet);
            CreateMockDataRow(sheet, 4, "15/11/2023", "Mock Description 1", "100.00", "1,000.00");
            CreateMockDataRow(sheet, 5, "01/12/2023", "Mock Description 2", "200.00", "1,200.00");
            CreateMockDataRow(sheet, 6, "15/12/2023", "Mock Description 3", "300.00", "1,500.00");
            CreateMockDataRow(sheet, 7, "01/01/2024", "Mock Description 4", "400.00", "1,900.00");

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act  
            var result = IngOperationsReader.GetIngOperations(memoryStream, TestMonth, TestYear);

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, operation =>
            {
                Assert.Equal(TestMonth, operation.Date.Month);
                Assert.Equal(TestYear, operation.Date.Year);
            });
        }

        [Fact]
        public void GetIngOperations_ForSpecificMonth_ShouldThrowExceptionForInvalidDate()
        {
            // Arrange  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            CreateHeaderRows(sheet);
            CreateMockDataRow(sheet, 4, "31/31/2023", "Mock Description 1", "100.00", "1,000.00");

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act & Assert  
            Assert.Throws<FormatException>(() => IngOperationsReader.GetIngOperations(memoryStream, TestMonth, TestYear));
        }

        [Fact]
        public void GetIngOperations_ForSpecificMonth_ShouldThrowExceptionForInvalidAmount()
        {
            // Arrange  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            CreateHeaderRows(sheet);
            CreateMockDataRow(sheet, 4, "01/01/2023", "Mock Description 1", "1.000,00", "2,000.00");

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act & Assert  
            Assert.Throws<FormatException>(() => IngOperationsReader.GetIngOperations(memoryStream, TestMonth, TestYear));
        }

        [Fact]
        public void GetIngOperations_ForSpreadsheetWithoutHeader_ShouldThrowInvalidOperationException()
        {
            // Arrange  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            sheet.CreateRow(0);
            sheet.CreateRow(1);

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act & Assert  
            Assert.Throws<InvalidOperationException>(() => IngOperationsReader.GetIngOperations(memoryStream, TestMonth, TestYear));
        }

        private static MemoryStream WriteWorkbookToMemoryStream(HSSFWorkbook workbook)
        {
            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private static void CreateHeaderRows(ISheet sheet)
        {
            for (int i = 0; i < 3; i++)
            {
                sheet.CreateRow(i);
            }

            CreateHeaderRow(sheet, 3);
        }

        private static void CreateHeaderRow(ISheet sheet, int rowNumber)
        {
            var headerRow = sheet.CreateRow(rowNumber);
            var headers = new[]
            {
                    "F. VALOR", "CATEGORÍA", "SUBCATEGORÍA", "DESCRIPCIÓN", "COMENTARIO", "IMAGEN", "IMPORTE (€)", "SALDO (€)"
                };

            for (int i = 0; i < headers.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(headers[i]);
            }
        }

        private static void CreateMockDataRow(ISheet sheet, int rowIndex, string date, string descripcion, string importe, string saldo)
        {
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue(date);   
            row.CreateCell(1).SetCellValue("Categoría");   
            row.CreateCell(2).SetCellValue("Subcategoría"); 
            row.CreateCell(3).SetCellValue(descripcion);   
            row.CreateCell(4).SetCellValue(string.Empty);  
            row.CreateCell(5).SetCellValue("No");  
            row.CreateCell(6).SetCellValue(importe);   
            row.CreateCell(7).SetCellValue(saldo);  
        }
    }
}
