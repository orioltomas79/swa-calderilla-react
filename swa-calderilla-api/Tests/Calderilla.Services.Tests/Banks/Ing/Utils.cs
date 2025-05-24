using NPOI.SS.UserModel;

namespace Calderilla.Services.Tests.Banks.Ing
{
    public class Utils
    {
        public static void CreateHeaderRows(ISheet sheet)
        {
            for (int i = 0; i < 3; i++)
            {
                sheet.CreateRow(i);
            }

            CreateHeaderRow(sheet, 3);
        }

        public static void CreateHeaderRow(ISheet sheet, int rowNumber)
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

        public static void CreateMockDataRow(ISheet sheet, int rowIndex, string date, string descripcion, string importe, string saldo)
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
