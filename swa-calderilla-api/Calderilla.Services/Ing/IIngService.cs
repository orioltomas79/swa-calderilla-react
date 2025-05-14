using NPOI.HSSF.UserModel;
using static Calderilla.Services.Ing.IngService;

namespace Calderilla.Services.Ing
{
    public interface IIngService
    {
        GetBankExtractDataResult GetBankExtractData(HSSFWorkbook workbook, int month, int year);
    }
}
