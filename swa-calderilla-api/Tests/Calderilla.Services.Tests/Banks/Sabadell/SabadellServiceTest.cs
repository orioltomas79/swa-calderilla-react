using Calderilla.Services.Banks.Sabadell;

namespace Calderilla.Services.Tests.Banks.Sabadell
{
    public class SabadellServiceTest
    {
        private const int TestMonth = 6;
        private const int TestYear = 2024;

        private readonly SabadellService _sabadellService;

        public SabadellServiceTest()
        {
            _sabadellService = new SabadellService();
        }

        [Fact]
        public void GetBankExtractData_ShouldReturnRawDataAndOperations()
        {
            // Arrange
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(Utils.CreateFakePipeRow("25/05/2024", "COMPRA TARJ. A", "-100.00", "2000.00", "5402__0037"));
            sb.AppendLine(Utils.CreateFakePipeRow("03/06/2024", "COMPRA TARJ. B", "-200.00", "2200.00", "5402__0037"));
            sb.AppendLine(Utils.CreateFakePipeRow("23/06/2024", "COMPRA TARJ. C", "-300.00", "2500.00", "5402__0037"));
            sb.AppendLine(Utils.CreateFakePipeRow("28/07/2024", "COMPRA TARJ. D", "-400.00", "2900.00", "5402__0037"));
            var extract = sb.ToString();

            // Act
            var result = _sabadellService.GetBankExtractData(extract, TestMonth, TestYear);

            // Assert
            Assert.NotNull(result.RawData);
            Assert.Equal(extract, result.RawData);

            Assert.NotNull(result.Operations);
            Assert.Equal(2, result.Operations.Count);
            Assert.All(result.Operations, operation =>
            {
                Assert.Equal(TestMonth, operation.OperationDate.Month);
                Assert.Equal(TestYear, operation.OperationDate.Year);
            });

            var firstOperation = result.Operations.First();
            Assert.Equal(new DateTime(2024, 6, 3), firstOperation.OperationDate);
            Assert.Equal("COMPRA TARJ. B", firstOperation.Description);
            Assert.Equal(-200.00m, firstOperation.Amount);
            Assert.Equal(2200.00m, firstOperation.Balance);
            Assert.Equal("5402__0037", firstOperation.Payer);
        }
    }
}
