using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public interface IOperationsRepository
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month);

        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year);

        Task<IEnumerable<Operation>> GetOperationsLast12MonthsAsync(string userId, Guid currentAccount, int year, int month);

        Task SaveOperationsAsync(IEnumerable<Operation> operations, string userId, Guid currentAccount, int year, int month);
    }
}
