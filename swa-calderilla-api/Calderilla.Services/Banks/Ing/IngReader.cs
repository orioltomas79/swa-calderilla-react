using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Calderilla.Services.Banks.Ing
{
    public class IngReader
    {
        public static ExtractIngDataResult ExtractData(HSSFWorkbook workbook, int month, int year)
        {
            var operationsList = new List<IngOperation>();
            var sheet = workbook.GetSheetAt(0);
            var headerRowNum = GetHeaderRowNum(sheet);
            var rawList = new List<string>();

            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;

                var rowValues = ExtractRowValues(row);
                rawList.Add(string.Join("|", rowValues));

                if (i > headerRowNum)
                {
                    var ingOperation = new IngOperation(row);
                    if (ingOperation.Date.Month == month && ingOperation.Date.Year == year)
                    {
                        operationsList.Add(ingOperation);
                    }
                }
            }

            return new ExtractIngDataResult
            {
                RawData = rawList,
                Operations = operationsList
            };
        }

        public class ExtractIngDataResult
        {
            public required List<string> RawData { get; set; }
            public required List<IngOperation> Operations { get; set; }
        }

        private static int GetHeaderRowNum(ISheet sheet)
        {
            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row != null && row.Cells.Count >= 7 && IsTheHeaderRow(row))
                {
                    return i;
                }
            }

            throw new InvalidOperationException("The header row was not found in the spreadsheet.");
        }

        private static bool IsTheHeaderRow(IRow row)
        {
            for (int j = 0; j < 5; j++)
            {
                var cell = row.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                {
                    return false;
                }
            }
            return true;
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
