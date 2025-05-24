using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public class OperationsRepository(IBlobRepository blobRepository) : IOperationsRepository
    {
        private readonly IBlobRepository _blobRepository = blobRepository;

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month)
        {
            return await _blobRepository.ReadListAsync<Operation>(GetBlobName(userId, currentAccount, year, month)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year)
        {
            var tasks = new List<Task<List<Operation>>>();

            for (int month = 1; month <= 12; month++)
            {
                tasks.Add(_blobRepository.ReadListAsync<Operation>(GetBlobName(userId, currentAccount, year, month)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.SelectMany(ops => ops);
        }

        public async Task<IEnumerable<Operation>> GetOperationsLast12MonthsAsync(string userId, Guid currentAccount, int year, int month)
        {
            var tasks = new List<Task<List<Operation>>>();

            var endDate = new DateTime(year, month, 1);
            for (int i = 1; i <= 12; i++)
            {
                var date = endDate.AddMonths(-i);
                tasks.Add(_blobRepository.ReadListAsync<Operation>(GetBlobName(userId, currentAccount,  date.Year, date.Month)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.SelectMany(ops => ops);
        }

        public async Task SaveOperationsAsync(IEnumerable<Operation> operations, string userId, Guid currentAccount, int year, int month)
        {
            await _blobRepository.WriteListAsync(GetBlobName(userId, currentAccount, year, month), operations.ToList()).ConfigureAwait(false);
        }

        private static string GetBlobName(string userId, Guid currentAccount, int year, int month)
        {
            return $"{userId}/{currentAccount}/{year}/{month}.json";
        }
    }
}
