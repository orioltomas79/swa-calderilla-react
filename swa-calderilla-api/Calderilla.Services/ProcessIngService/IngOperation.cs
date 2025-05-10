using System.Globalization;
using NPOI.SS.UserModel;

namespace Calderilla.Services.ProcessIngService
{
    public class IngOperation
    {
        public IngOperation(IRow row)
        {
            var cultureUS = new CultureInfo("en-US");
            var cultureES = new CultureInfo("es-ES");

            Date = DateOnly.Parse(row.GetCell(0).ToString()!, cultureES);
            Description = row.GetCell(3).ToString()!;
            Amount = decimal.Parse(row.GetCell(6).ToString()!, cultureUS);
            Total = decimal.Parse(row.GetCell(7).ToString()!, cultureUS);
        }

        public DateOnly Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}
