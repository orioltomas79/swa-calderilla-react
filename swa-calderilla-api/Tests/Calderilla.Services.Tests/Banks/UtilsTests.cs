using Calderilla.Services.Banks;

namespace Calderilla.Services.Tests.Banks
{
    public class UtilsTests
    {
        [Theory]
        [InlineData("0", 0)]
        [InlineData("", 0)]
        [InlineData("  987,65  ", 987.65)]
        [InlineData("2,000.00", 2000)]
        [InlineData("1234.56", 1234.56)]
        [InlineData("1,234.56", 1234.56)]
        [InlineData("-1234.56", -1234.56)]
        [InlineData("-1,234.56", -1234.56)]
        [InlineData("2.000,00", 2000)]
        [InlineData("1.234,56", 1234.56)]
        [InlineData("1234,56", 1234.56)]
        [InlineData("-1.234,56", -1234.56)]
        [InlineData("-1234,56", -1234.56)]
        public void ParseDecimalAutoDetectCulture_ParsesVariousFormats(string input, decimal expected)
        {
            var result = Utils.ParseDecimalAutoDetectCulture(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseDecimalAutoDetectCulture_InvalidInput_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => Utils.ParseDecimalAutoDetectCulture("abc"));
            Assert.Throws<FormatException>(() => Utils.ParseDecimalAutoDetectCulture("12,34.56.78"));
        }
    }
}