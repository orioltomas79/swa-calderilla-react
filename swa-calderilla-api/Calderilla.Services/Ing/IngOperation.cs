using System.Globalization;
using NPOI.SS.UserModel;

namespace Calderilla.Services.Ing
{
    public class IngOperation
    {
        private static readonly CultureInfo CultureUS = new("en-US");
        private static readonly CultureInfo CultureES = new("es-ES");

        public IngOperation(IRow row)
        {
            Date = DateOnly.Parse(row.GetCell(0).ToString()!, CultureES);
            Description = row.GetCell(3).ToString()!;
            Amount = decimal.Parse(row.GetCell(6).ToString()!, CultureES);
            Total = decimal.Parse(row.GetCell(7).ToString()!, CultureES);
        }

        public DateOnly Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}
