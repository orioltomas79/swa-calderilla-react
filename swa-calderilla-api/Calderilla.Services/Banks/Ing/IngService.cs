using Calderilla.Services.Banks;
using NPOI.HSSF.UserModel;

namespace Calderilla.Services.Banks.Ing
{
    public class IngService : IIngService
    {
        public GetBankExtractResult GetBankExtractData(HSSFWorkbook workbook, int month, int year)
        {
            var extractedData = IngReader.ExtractData(workbook, month, year);

            var operations = IngOperationMapper.MapIngOperationsToDomainOperations(extractedData.Operations);

            return new GetBankExtractResult
            {
                RawData = extractedData.CsvData,
                Operations = operations
            };
        }
    }
}
