using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public class OperationsRepository(IBlobRepository blobRepository) : IOperationsRepository
    {
        private readonly IBlobRepository _blobRepository = blobRepository;

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month)
        {
            return await _blobRepository.ReadListAsync<Operation>(GetBlobNameMonth(userId, currentAccount, year, month)).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year)
        {
            var yearBlobName = GetBlobNameYear(userId, currentAccount, year);
            if (await _blobRepository.ExistsAsync(yearBlobName).ConfigureAwait(false))
            {
                return await _blobRepository.ReadListAsync<Operation>(yearBlobName).ConfigureAwait(false);
            }

            var tasks = new List<Task<List<Operation>>>();
            for (int month = 1; month <= 12; month++)
            {
                tasks.Add(_blobRepository.ReadListAsync<Operation>(GetBlobNameMonth(userId, currentAccount, year, month)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            await _blobRepository.WriteListAsync(yearBlobName, results.SelectMany(ops => ops).ToList()).ConfigureAwait(false);
            return results.SelectMany(ops => ops);
        }

        public async Task<IEnumerable<Operation>> GetOperationsLast12MonthsAsync(string userId, Guid currentAccount, int year, int month)
        {
            var tasks = new List<Task<List<Operation>>>();

            var endDate = new DateTime(year, month, 1);
            for (int i = 1; i <= 12; i++)
            {
                var date = endDate.AddMonths(-i);
                tasks.Add(_blobRepository.ReadListAsync<Operation>(GetBlobNameMonth(userId, currentAccount, date.Year, date.Month)));
            }

            var results = await Task.WhenAll(tasks).ConfigureAwait(false);

            return results.SelectMany(ops => ops);
        }

        public async Task SaveOperationsAsync(IEnumerable<Operation> operations, string userId, Guid currentAccount, int year, int month)
        {
            // Delete the year blob if it exists
            var yearBlobName = GetBlobNameYear(userId, currentAccount, year);
            await _blobRepository.DeleteAsync(yearBlobName).ConfigureAwait(false);
            await _blobRepository.WriteListAsync(GetBlobNameMonth(userId, currentAccount, year, month), operations.ToList()).ConfigureAwait(false);
        }

        private static string GetBlobNameMonth(string userId, Guid currentAccount, int year, int month)
        {
            return $"{userId}/{currentAccount}/{year}/{month}.json";
        }

        private static string GetBlobNameYear(string userId, Guid currentAccount, int year)
        {
            return $"{userId}/{currentAccount}/{year}.json";
        }
    }
}
