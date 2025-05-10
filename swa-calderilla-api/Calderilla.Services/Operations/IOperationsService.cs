using Calderilla.Domain;

namespace Calderilla.Services.Operations
{
    public interface IOperationsService
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month);
    }
}
