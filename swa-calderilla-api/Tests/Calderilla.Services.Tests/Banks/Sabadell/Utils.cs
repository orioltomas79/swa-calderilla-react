namespace Calderilla.Services.Tests.Banks.Sabadell;

public class Utils
{
    public static string CreateFakePipeRow(string date, string description, string amount, string total, string payer)
    {
        return $"{date}|{description}|{date}|{amount}|{total}||{payer}";
    }
}
