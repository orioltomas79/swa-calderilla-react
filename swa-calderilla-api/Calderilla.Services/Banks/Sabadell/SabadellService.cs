using Calderilla.Services.Banks;

namespace Calderilla.Services.Bank.Sabadell
{
    public class SabadellService : ISabadellService
    {
        public GetBankExtractResult GetBankExtractData(string extract, int month, int year)
        {
            return new GetBankExtractResult()
            {
                CsvData = string.Empty,
                Operations = new List<Domain.Operation>()
            };
        }
    }
}
