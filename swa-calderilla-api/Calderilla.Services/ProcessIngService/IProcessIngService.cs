using Calderilla.Domain;

namespace Calderilla.Services.ProcessIngService
{
    public interface IProcessIngService
    {
        Task<IEnumerable<Operation>> GetOperationsAsync(string userId, Guid currentAccount, int year, int month);
    }
}
