using NPOI.HSSF.UserModel;
using Calderilla.Services.Banks.Ing;

namespace Calderilla.Services.Tests.Banks.Ing
{
    public class IngReaderTests
    {
        private const int TestMonth = 12;
        private const int TestYear = 2023;

        [Fact]
        public void ExtractData_ForSpecificMonth_ShouldReturnFilteredOperationsAndRawData()
        {
            // Arrange  
            using var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            Utils.CreateHeaderRows(sheet);
            Utils.CreateMockDataRow(sheet, 4, "15/11/2023", "Mock Description 1", "100,00", "1.000,00");
            Utils.CreateMockDataRow(sheet, 5, "01/12/2023", "Mock Description 2", "200,00", "1.200,00");
            Utils.CreateMockDataRow(sheet, 6, "15/12/2023", "Mock Description 3", "300,00", "1.500,00");
            Utils.CreateMockDataRow(sheet, 7, "01/01/2024", "Mock Description 4", "400,00", "1.900,00");

            // Act  
            var result = IngReader.ExtractData(workbook, TestMonth, TestYear);

            // Assert  
            Assert.NotNull(result.RawData);
            var rawLines = result.RawData;
            Assert.Equal("F. VALOR|CATEGORÍA|SUBCATEGORÍA|DESCRIPCIÓN|COMENTARIO|IMAGEN|IMPORTE (€)|SALDO (€)", rawLines[3]);
            Assert.Equal("15/11/2023|Categoría|Subcategoría|Mock Description 1||No|100,00|1.000,00", rawLines[4]);

            Assert.NotNull(result.Operations);
            Assert.Equal(2, result.Operations.Count);
            Assert.All(result.Operations, operation =>
            {
                Assert.Equal(TestMonth, operation.Date.Month);
                Assert.Equal(TestYear, operation.Date.Year);
            });

            var firstOperation = result.Operations.First();
            Assert.Equal("01/12/2023", firstOperation.Date.ToString("dd/MM/yyyy"));
            Assert.Equal("Mock Description 2", firstOperation.Description);
            Assert.Equal(200, firstOperation.Amount);
            Assert.Equal(1200, firstOperation.Total);
        }

        [Fact]
        public void ExtractData_ForSpecificMonth_ShouldThrowExceptionForInvalidDate()
        {
            // Arrange  
            using var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            Utils.CreateHeaderRows(sheet);
            Utils.CreateMockDataRow(sheet, 4, "31/31/2023", "Mock Description 1", "100.00", "1.000,00");

            // Act & Assert  
            Assert.Throws<FormatException>(() => IngReader.ExtractData(workbook, TestMonth, TestYear));
        }

        [Fact]
        public void ExtractData_ForSpreadsheetWithoutHeader_ShouldThrowInvalidOperationException()
        {
            // Arrange  
            using var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            sheet.CreateRow(0);
            sheet.CreateRow(1);

            // Act & Assert  
            Assert.Throws<InvalidOperationException>(() => IngReader.ExtractData(workbook, TestMonth, TestYear));
        }
    }
}
