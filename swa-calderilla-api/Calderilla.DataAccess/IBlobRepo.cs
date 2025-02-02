using Calderilla.Domain;

namespace Calderilla.DataAccess
{
    public interface IBlobRepo
    {
        string GetMessage();

        List<Operation> GetOperations(int month, int year);
    }
}
