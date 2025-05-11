using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Calderilla.Services.ProcessIngService;

namespace Calderilla.Services.Tests
{
    public class ExcelToCsvTests
    {
        [Fact]
        public void GetCsv_ShouldConvertExcelToCsvCorrectly()
        {
            // Arrange
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            CreateMockDataRow(sheet, 0, "Value1", "Value2");
            CreateMockDataRow(sheet, 1, "Data1", "Data2");

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act
            var result = ExcelToCsv.GetCsv(memoryStream);

            // Assert
            var expectedCsv = "Value1,Value2\r\nData1,Data2\r\n";
            Assert.Equal(expectedCsv, result);
        }

        [Fact]
        public void GetCsv_ShouldHandleNullCellsGracefully()
        {
            // Arrange
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue("Value1");
            row.CreateCell(1); // Null cell
            row.CreateCell(2).SetCellValue("Value3");

            using var memoryStream = WriteWorkbookToMemoryStream(workbook);

            // Act
            var result = ExcelToCsv.GetCsv(memoryStream);

            // Assert
            var expectedCsv = "Value1,,Value3\r\n";
            Assert.Equal(expectedCsv, result);
        }

        private static MemoryStream WriteWorkbookToMemoryStream(HSSFWorkbook workbook)
        {
            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private static void CreateMockDataRow(ISheet sheet, int rowIndex, params string[] values)
        {
            var row = sheet.CreateRow(rowIndex);
            for (int i = 0; i < values.Length; i++)
            {
                row.CreateCell(i).SetCellValue(values[i]);
            }
        }
    }
}
