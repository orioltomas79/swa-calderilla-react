namespace Calderilla.Services.ProcessIngService
{
    public class ProcessIngService
    {
        public static List<Domain.Operation> ProcessIngExtract(Stream stream, int month, int year)
        {
            // Get all ing operations for the given month and year
            var ingOperations = IngOperationsReader.GetIngOperations(stream, month, year);

            // Map ing operations to operations

            // Enrich operations based on historical data

            return new List<Domain.Operation>();
        }
    }
}
