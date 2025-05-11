namespace Calderilla.Services.Ing
{
    public static class IngOperationMapper
    {
        public static List<Domain.Operation> MapIngOperationsToDomainOperations(IEnumerable<IngOperation> ingOperations)
        {
            var operations = new List<Domain.Operation>();
            int monthOperationNumber = 1;

            foreach (var ingOperation in ingOperations)
            {
                operations.Add(MapIngOperationToDomainOperation(ingOperation, monthOperationNumber++));
            }

            return operations;
        }

        private static Domain.Operation MapIngOperationToDomainOperation(IngOperation ingOperation, int monthOperationNumber)
        {
            var operationDate = ingOperation.Date.ToDateTime(TimeOnly.MinValue); // Convert DateOnly to DateTime

            return new Domain.Operation
            {
                Id = Guid.NewGuid(),
                MonthOperationNumber = monthOperationNumber,
                OperationDate = operationDate,
                Description = ingOperation.Description,
                ValueDate = operationDate,
                Amount = ingOperation.Amount,
                Balance = ingOperation.Total,
                Payer = string.Empty,
                Ignore = false,
                Type = null,
                Notes = null,
                Reviewed = false
            };
        }
    }
}
