using System.Globalization;

namespace Calderilla.Services.Banks.Sabadell;

public class SabadellOperation
{
    private static readonly CultureInfo CultureUS = new("en-US");
    private static readonly CultureInfo CultureES = new("es-ES");

    public SabadellOperation(string row)
    {
        var columns = row.Split('|');

        Date = DateOnly.Parse(columns[0], CultureES);
        Description = columns[1];
        Amount = decimal.Parse(columns[3], CultureUS);
        Total = decimal.Parse(columns[4], CultureUS);
        Payer = columns[6];
    }

    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Total { get; set; }
    public string Payer { get; set; }
}
