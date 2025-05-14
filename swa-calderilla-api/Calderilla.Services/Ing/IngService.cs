using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Calderilla.Services.Ing
{
    public class IngService : IIngService
    {
        public GetBankExtractDataResult GetBankExtractData(HSSFWorkbook workbook, int month, int year)
        {
            var extractedData = IngReader.ExtractData(workbook, month, year);

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
