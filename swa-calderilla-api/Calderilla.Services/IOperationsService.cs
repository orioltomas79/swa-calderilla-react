using Calderilla.Domain;

namespace Calderilla.Services
{
    public interface IOperationsService
    {
        public List<Operation> GetOperations(int month, int year);
    }
}
