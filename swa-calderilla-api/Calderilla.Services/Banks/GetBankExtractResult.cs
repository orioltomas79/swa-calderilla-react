namespace Calderilla.Services.Banks
{
    public class GetBankExtractResult
    {
        public required List<string> RawData { get; set; }
        public required List<Domain.Operation> Operations { get; set; }
    }
}