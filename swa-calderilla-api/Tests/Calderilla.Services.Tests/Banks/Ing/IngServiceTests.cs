using Calderilla.Services.Banks.Ing;
using NPOI.HSSF.UserModel;

namespace Calderilla.Services.Tests.Banks.Ing
{
    public class IngServiceTests
    {
        private const int TestMonth = 12;
        private const int TestYear = 2023;
        
        private readonly IngService _ingReaderService;

        public IngServiceTests()
        {
            _ingReaderService = new IngService();
        }

        [Fact]
        public void GetBankExtractData_ShouldReturnRawAndOperations()
        {
            // Arrange
            using var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            Utils.CreateHeaderRows(sheet);
            Utils.CreateMockDataRow(sheet, 4, "01/12/2023", "Mock Description 1", "100,00", "1.000,00");
            Utils.CreateMockDataRow(sheet, 5, "15/12/2023", "Mock Description 2", "200,00", "1.200,00");

            // Act
            var result = _ingReaderService.GetBankExtractData(workbook, TestMonth, TestYear);

            // Assert
            Assert.NotNull(result.RawData);
            var rawLines = result.RawData;
            Assert.Equal("F. VALOR|CATEGORÍA|SUBCATEGORÍA|DESCRIPCIÓN|COMENTARIO|IMAGEN|IMPORTE (€)|SALDO (€)", rawLines[3]);
            Assert.Equal("01/12/2023|Categoría|Subcategoría|Mock Description 1||No|100,00|1.000,00", rawLines[4]);

            Assert.NotNull(result.Operations);
            Assert.Equal(2, result.Operations.Count);
            Assert.All(result.Operations, operation =>
            {
                Assert.Equal(TestMonth, operation.OperationDate.Month);
                Assert.Equal(TestYear, operation.OperationDate.Year);
            });

            var firstOperation = result.Operations.First();

            Assert.Equal(1, firstOperation.MonthOperationNumber);
            Assert.Equal("01/12/2023", firstOperation.OperationDate.ToString("dd/MM/yyyy"));
            Assert.Equal("Mock Description 1", firstOperation.Description);
            Assert.Equal(100, firstOperation.Amount);
            Assert.Equal(1000, firstOperation.Balance);
        }
    }
}
