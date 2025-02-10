using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public interface IOperationsRepository
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month);
    }
}
