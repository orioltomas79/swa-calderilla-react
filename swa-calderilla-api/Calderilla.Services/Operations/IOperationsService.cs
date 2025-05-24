using Calderilla.Domain;

namespace Calderilla.Services.Operations
{
    public interface IOperationsService
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month);

        Task EnrichOperationTypeAsync(string userId, Guid currentAccount, IEnumerable<Operation> operations, int year, int month);

        Task SaveOperationAsync(IEnumerable<Operation> operations, string userId, Guid currentAccount, int year, int month);
    }
}
