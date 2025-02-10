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

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month)
        {
            return await _blobRepository.ReadListAsync<Operation>(GetBlobName(userId, currentAccount, year, month)).ConfigureAwait(false);
        }

        private static string GetBlobName(string userId, Guid currentAccount, int year, int month)
        {
            return $"{userId}/{currentAccount}/{year}/{month}.json";
        }
    }
}
