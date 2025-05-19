namespace Calderilla.Services.Banks.Sabadell
{
    public class SabadellService : ISabadellService
    {
        public GetBankExtractResult GetBankExtractData(string pipeSeparatedBankExtract, int month, int year)
        {

            var extractedData = SabadellReader.ExtractData(pipeSeparatedBankExtract, month, year);

            var operations = SabadellOperationMapper.MapSabadellOperationsToDomainOperations(extractedData.Operations);

            return new GetBankExtractResult()
            {
                RawData = extractedData.RawData,
                Operations = operations
            };
        }
    }
}
