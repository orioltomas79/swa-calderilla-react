namespace Calderilla.Services.Banks.Sabadell;

public class SabadellOperationMapper
{
    public static List<Domain.Operation> MapSabadellOperationsToDomainOperations(IEnumerable<SabadellOperation> sabadellOperations)
    {
        var operations = new List<Domain.Operation>();
        int monthOperationNumber = 1;

        foreach (var sabadellOperation in sabadellOperations)
        {
            operations.Add(MapSabadellOperationToDomainOperation(sabadellOperation, monthOperationNumber++));
        }

        return operations;
    }

    private static Domain.Operation MapSabadellOperationToDomainOperation(SabadellOperation sabadellOperation, int monthOperationNumber)
    {
        var operationDate = sabadellOperation.Date.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime

        return new Domain.Operation
        {
            Id = Guid.NewGuid(),
            MonthOperationNumber = monthOperationNumber,
            OperationDate = operationDate,
            Description = sabadellOperation.Description,
            ValueDate = operationDate,
            Amount = sabadellOperation.Amount,
            Balance = sabadellOperation.Total,
            Payer = sabadellOperation.Payer,
            Ignore = false,
            Type = null,
            Notes = null,
            Reviewed = false
        };
    }
}
