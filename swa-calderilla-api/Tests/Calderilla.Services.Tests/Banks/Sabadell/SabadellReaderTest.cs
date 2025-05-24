using Calderilla.Services.Banks.Sabadell;
using Calderilla.Test.Utils;

namespace Calderilla.Services.Tests.Banks.Sabadell;

public class SabadellReaderTest
{
    private const int TestMonth = 6;
    private const int TestYear = 2024;

    [Fact]
    public void ExtractData_ReturnsOperationsForGivenMonthAndYear()
    {
        // Arrange
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(Utils.CreateFakePipeRow("25/05/2024", "COMPRA TARJ. A", "-100.00", "2000.00", "5402__0037"));
        sb.AppendLine(Utils.CreateFakePipeRow("03/06/2024", "COMPRA TARJ. B", "-200.00", "2200.00", "5402__0037"));
        sb.AppendLine(Utils.CreateFakePipeRow("23/06/2024", "COMPRA TARJ. C", "-300.00", "2500.00", "5402__0037"));
        sb.Append(Utils.CreateFakePipeRow("28/07/2024", "COMPRA TARJ. D", "-400.00", "2900.00", "5402__0037"));
        var extract = sb.ToString();

        // Act
        var result = SabadellReader.ExtractData(extract, TestMonth, TestYear);

        // Assert
        Assert.NotNull(result);
        List<string> expectedRawData = extract.SplitLines().ToList();
        Assert.Equal(expectedRawData, result.RawData);

        Assert.Equal(2, result.Operations.Count);
        Assert.All(result.Operations, operation =>
        {
            Assert.Equal(TestMonth, operation.Date.Month);
            Assert.Equal(TestYear, operation.Date.Year);
        });

        var firstOperation = result.Operations.First();
        Assert.Equal(new DateOnly(2024, 6, 3), firstOperation.Date);
        Assert.Equal("COMPRA TARJ. B", firstOperation.Description);
        Assert.Equal(-200.00m, firstOperation.Amount);
        Assert.Equal(2200.00m, firstOperation.Total);
        Assert.Equal("5402__0037", firstOperation.Payer);
    }

    [Fact]
    public void ExtractData_ForSpecificMonth_ShouldThrowExceptionForInvalidDate()
    {
        // Arrange  
        var extract = Utils.CreateFakePipeRow("31/31/2023", "COMPRA TARJ. A", "-100.00", "2000.00", "5402__0037");

        // Act & Assert  
        Assert.Throws<FormatException>(() => SabadellReader.ExtractData(extract, TestMonth, TestYear));
    }
}
