using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public interface IOperationsRepository
    {
        Task AddOperationAsync(string userId, Operation operation);

        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, int year, int month);
    }
}
