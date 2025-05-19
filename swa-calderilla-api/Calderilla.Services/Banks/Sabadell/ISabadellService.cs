namespace Calderilla.Services.Banks.Sabadell
{
    public interface ISabadellService
    {
        GetBankExtractResult GetBankExtractData(string pipeSeparatedBankExtract, int month, int year);
    }
}
