using System.Globalization;
using NPOI.SS.UserModel;

namespace Calderilla.Services.Banks.Ing
{
    public class IngOperation
    {
        private static readonly CultureInfo CultureES = new("es-ES");

        public IngOperation(IRow row)
        {
            Date = DateOnly.Parse(row.GetCell(0).ToString()!, CultureES);
            Description = row.GetCell(3).ToString()!;
            Amount = Utils.ParseDecimalAutoDetectCulture(row.GetCell(5).ToString()!);
            Total = Utils.ParseDecimalAutoDetectCulture(row.GetCell(6).ToString()!);
        }

        public DateOnly Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}
