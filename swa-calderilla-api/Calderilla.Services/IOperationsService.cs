using Calderilla.Domain;

namespace Calderilla.Services
{
    public interface IOperationsService
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, int year, int month);

        Task AddOperationAsync(string userId, Operation operation);
    }
}
