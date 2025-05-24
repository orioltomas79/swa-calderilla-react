using Calderilla.DataAccess;
using Calderilla.Domain;

namespace Calderilla.Services.Operations
{
    public class OperationsService(IOperationsRepository operationsRepository) : IOperationsService
    {
        private readonly IOperationsRepository _operationsRepository = operationsRepository ?? throw new ArgumentNullException(nameof(operationsRepository));

        public Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month)
        {
            ValidateOperationsParameters(userId, year, month);
            return _operationsRepository.GetOperationsAsync(userId, currentAccount, year, month);
        }

        public async Task SaveOperationAsync(IEnumerable<Operation> operations, string userId, Guid currentAccount, int year, int month)
        {
            ValidateOperationsParameters(userId, year, month);
            ArgumentNullException.ThrowIfNull(operations);

            await _operationsRepository.SaveOperationsAsync(operations, userId, currentAccount, year, month).ConfigureAwait(false);
        }

        public async Task EnrichOperationTypeAsync(string userId, Guid currentAccount, IEnumerable<Operation> operations, int year, int month)
        {
            var last12MonthsOperations = await _operationsRepository.GetOperationsLast12MonthsAsync(userId, currentAccount, year, month).ConfigureAwait(false);
            foreach (var operation in operations)
            {
                EnrichType(operation, last12MonthsOperations);
            }
        }

        private static void ValidateOperationsParameters(string userId, int year, int month)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            if (year < 1 || year > 9999) throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1 and 9999.");
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
        }

        private static void EnrichType(Operation operation, IEnumerable<Operation> last12MonthsOperations)
        {
            var matchingOperations = last12MonthsOperations
                .Where(o => string.Equals(o.Description, operation.Description, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchingOperations.Count != 0)
            {
                operation.Type = matchingOperations
                    .GroupBy(o => o.Type)
                    .OrderByDescending(g => g.Count())
                    .First()
                    .Key;
            }
        }
    }
}
