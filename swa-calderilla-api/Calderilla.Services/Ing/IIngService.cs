using static Calderilla.Services.Ing.IngService;

namespace Calderilla.Services.Ing
{
    public interface IIngService
    {
        GetBankExtractDataResult GetBankExtractData(Stream stream, int month, int year);
    }
}
