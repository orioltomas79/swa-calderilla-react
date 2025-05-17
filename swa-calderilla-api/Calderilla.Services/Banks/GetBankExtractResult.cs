namespace Calderilla.Services.Banks
{
    public class GetBankExtractResult
    {
        public required string CsvData { get; set; }
        public required List<Domain.Operation> Operations { get; set; }
    }
}