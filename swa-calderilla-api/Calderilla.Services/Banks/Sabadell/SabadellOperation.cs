using System.Globalization;

namespace Calderilla.Services.Banks.Sabadell;

public class SabadellOperation
{
    private static readonly CultureInfo CultureES = new("es-ES");

    public SabadellOperation(string row)
    {
        var columns = row.Split('|');

        Date = DateOnly.Parse(columns[0], CultureES);
        Description = columns[1];
        Amount = Utils.ParseDecimalAutoDetectCulture(columns[3]);
        Total = Utils.ParseDecimalAutoDetectCulture(columns[4]);
        Payer = columns[6];
    }

    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Total { get; set; }
    public string Payer { get; set; }
}
