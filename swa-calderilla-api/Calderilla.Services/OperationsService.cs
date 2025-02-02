using Calderilla.DataAccess;
using Calderilla.Domain;

namespace Calderilla.Services
{
    public class OperationsService : IOperationsService
    {
        private readonly IBlobRepo _blobRepo;

        public OperationsService(IBlobRepo blobRepo)
        {
            _blobRepo = blobRepo;
        }

        public List<Operation> GetOperations(int month, int year)
        {
            return _blobRepo.GetOperations(month, year);
        }
    }
}
