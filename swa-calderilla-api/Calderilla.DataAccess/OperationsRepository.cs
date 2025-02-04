using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public class OperationsRepository : IOperationsRepository
    {
        private readonly IBlobRepository _blobRepository;

        public OperationsRepository(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        public async Task AddOperationAsync(string userId, Operation operation)
        {
            var blobName = GetBlobName(userId, operation.OperationDate.Year, operation.OperationDate.Month);

            var operations = await _blobRepository.ReadListAsync<Operation>(blobName).ConfigureAwait(false);
            operations.Add(operation);
            await _blobRepository.WriteListAsync(blobName, operations).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, int year, int month)
        {
            return await _blobRepository.ReadListAsync<Operation>(GetBlobName(userId, year, month)).ConfigureAwait(false);
        }

        private static string GetBlobName(string userId, int year, int month)
        {
            return $"{userId}/{year}/{month}.json";
        }
    }
}
