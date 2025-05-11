namespace Calderilla.Services.ProcessIngService
{
    public class ProcessIngService
    {
        public static (string, List<Domain.Operation>) ProcessIngExtract(Stream stream, int month, int year)
        {
            // Get all ing operations for the given month and year
            var ingOperations = IngOperationsReader.GetIngOperations(stream, month, year);

            // Get the content of the spreadsheet in a csv format
            var csvContent = ExcelToCsv.GetCsv(stream);

            // Map ing operations to operations

            // Enrich operations based on historical data

            return (csvContent, new List<Domain.Operation>());
        }
    }
}
