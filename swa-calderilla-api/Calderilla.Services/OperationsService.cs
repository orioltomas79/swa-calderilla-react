using Calderilla.DataAccess;
using Calderilla.Domain;

namespace Calderilla.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IOperationsRepository _operationsRepository;

        public OperationsService(IOperationsRepository operationsRepository)
        {
            _operationsRepository = operationsRepository ?? throw new ArgumentNullException(nameof(operationsRepository));
        }

        public Task AddOperationAsync(string userId, Operation operation)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            return operation == null
                ? throw new ArgumentNullException(nameof(operation))
                : _operationsRepository.AddOperationAsync(userId, operation);
        }

        public Task<IEnumerable<Operation>> GetOperationsAsync(string userId, int year, int month)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("User ID cannot be null or whitespace.", nameof(userId));
            if (year < 1 || year > 9999) throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1 and 9999.");
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");

            return _operationsRepository.GetOperationsAsync(userId, year, month);
        }
    }
}
