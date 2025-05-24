using System.Globalization;

namespace Calderilla.Services.Banks;

public class Utils
{
    public static decimal ParseDecimalAutoDetectCulture(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0m;

        input = input.Trim();

        // Determine which character is the decimal separator (the rightmost one)
        int lastComma = input.LastIndexOf(',');
        int lastDot = input.LastIndexOf('.');
        char decimalSep = lastComma > lastDot ? ',' : '.';
        char thousandSep = decimalSep == ',' ? '.' : ',';

        // Remove thousand separators
        input = input.Replace(thousandSep.ToString(), "");

        // Replace decimal separator with '.' for invariant parsing
        if (decimalSep != '.')
            input = input.Replace(decimalSep, '.');

        return decimal.Parse(input, CultureInfo.InvariantCulture);
    }
}
