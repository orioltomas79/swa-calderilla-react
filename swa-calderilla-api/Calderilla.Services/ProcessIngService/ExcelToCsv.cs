using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Calderilla.Services.ProcessIngService
{
    public class ExcelToCsv
    {
        public static string GetCsv(Stream stream)
        {
            using var workbook = new HSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            var stringBuilder = new StringBuilder();

            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;

                var rowValues = ExtractRowValues(row);
                stringBuilder.AppendLine(string.Join(",", rowValues));
            }

            return stringBuilder.ToString();
        }

        private static List<string> ExtractRowValues(IRow row)
        {
            var rowValues = new List<string>();
            for (int j = 0; j < row.LastCellNum; j++)
            {
                var cell = row.GetCell(j);
                rowValues.Add(cell?.ToString() ?? string.Empty);
            }
            return rowValues;
        }
    }
}
