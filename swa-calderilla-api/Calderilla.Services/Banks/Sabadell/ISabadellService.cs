using Calderilla.Services.Banks;

namespace Calderilla.Services.Bank.Sabadell
{
    public interface ISabadellService
    {
        GetBankExtractResult GetBankExtractData(string extractCsv, int month, int year);
    }
}
