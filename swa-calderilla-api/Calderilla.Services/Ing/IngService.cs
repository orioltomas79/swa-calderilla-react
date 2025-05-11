namespace Calderilla.Services.Ing
{
    public class IngService : IIngService
    {
        public GetBankExtractDataResult GetBankExtractData(Stream stream, int month, int year)
        {
            var extractedData = IngReader.ExtractData(stream, month, year);

            var operations = IngOperationMapper.MapIngOperationsToDomainOperations(extractedData.Operations);

            return new GetBankExtractDataResult
            {
                CsvData = extractedData.CsvData,
                Operations = operations
            };
        }

        public class GetBankExtractDataResult
        {
            public required string CsvData { get; set; }
            public required List<Domain.Operation> Operations { get; set; }
        }
    }
}
