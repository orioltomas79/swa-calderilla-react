using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Calderilla.Services.ProcessIngService
{
    public class IngOperationsReader
    {
        public static List<IngOperation> GetIngOperations(Stream stream, int month, int year)
        {
            using var workbook = new HSSFWorkbook(stream);
            return GetAllIngOperations(workbook.GetSheetAt(0), month, year);
        }

        private static List<IngOperation> GetAllIngOperations(ISheet sheet, int month, int year)
        {
            var operationsList = new List<IngOperation>();
            var headerRowNum = GetHeaderRowNum(sheet);

            for (int i = headerRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;

                var ingOperation = new IngOperation(row);
                if (ingOperation.Date.Month == month && ingOperation.Date.Year == year)
                {
                    operationsList.Add(ingOperation);
                }
            }

            return operationsList;
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
            for (int j = 0; j < 7; j++)
            {
                var cell = row.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
