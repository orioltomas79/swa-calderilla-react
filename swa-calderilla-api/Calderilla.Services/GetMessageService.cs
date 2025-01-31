using Calderilla.DataAccess;

namespace Calderilla.Services
{
    public class GetMessageService: IGetMessageService
    {
        private readonly IBlobRepo _blobRepo;

        public GetMessageService(IBlobRepo blobRepo)
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
