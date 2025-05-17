using NPOI.HSSF.UserModel;

namespace Calderilla.Services.Banks.Ing
{
    public interface IIngService
    {
        GetBankExtractResult GetBankExtractData(HSSFWorkbook workbook, int month, int year);
    }
}
