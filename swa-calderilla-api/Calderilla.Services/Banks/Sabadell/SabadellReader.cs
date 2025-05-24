namespace Calderilla.Services.Banks.Sabadell;

public class SabadellReader
{
    public static ExtractSabadellDataResult ExtractData(string pipeSeparatedBankExtract, int month, int year)
    {
        var operationsList = new List<SabadellOperation>();
        var textRawList = new List<string>();

        var rows = GetLines(pipeSeparatedBankExtract);

        foreach (var row in rows)
        {
            if (string.IsNullOrWhiteSpace(row) || row.Count(c => c == '|') != 6)
            {
                continue;
            }

            textRawList.Add(row);

            var sabadellOperation = new SabadellOperation(row);
            if (sabadellOperation.Date.Month == month && sabadellOperation.Date.Year == year)
            {
                operationsList.Add(sabadellOperation);
            }
        }

        return new ExtractSabadellDataResult()
        {
            RawData = textRawList,
            Operations = operationsList
        };
    }

    public class ExtractSabadellDataResult
    {
        public required List<string> RawData { get; set; }
        public required List<SabadellOperation> Operations { get; set; }
    }

    public static string[] GetLines(string text) => text.Split(["\r\n", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries);
}
