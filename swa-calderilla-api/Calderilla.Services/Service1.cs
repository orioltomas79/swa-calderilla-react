using Calderilla.DataAccess;

namespace Calderilla.Services
{
    public class Service1
    {
        private readonly IBlobRepo _blobRepo;

        public Service1(IBlobRepo blobRepo)
        {
            _blobRepo = blobRepo;
        }

        public string GetMessage(string userName)
        {
            var repoMessage = _blobRepo.GetMessage();
            return $"Welcome to Calderilla Service {userName}! {repoMessage}";
        }
    }
}
