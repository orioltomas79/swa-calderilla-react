using System.Text.Json;
using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public class BlobRepo : IBlobRepo
    {
        public string GetMessage() => "Hello from Blob repo!";

        public List<Operation> GetOperations(int month, int year)
        {
            var filePath = "sample.json";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }

            var json = File.ReadAllText(filePath);
            var operations = JsonSerializer.Deserialize<List<Operation>>(json);

            if (operations == null)
            {
                throw new InvalidOperationException("The file could not be deserialized.");
            }

            return operations.Where(op => op.OperationDate.Month == month && op.OperationDate.Year == year).ToList();
        }
    }
}
