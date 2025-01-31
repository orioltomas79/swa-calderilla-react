using Calderilla.DataAccess;
using Moq;

namespace Calderilla.Services.Tests
{
    public class GetMessageServiceShould
    {
        [Fact]
        public void GetMessage_WhenGetMessageIsCalled()
        {
            // Arrange
            var mockBlobRepo = new Mock<IBlobRepo>();
            mockBlobRepo.Setup(repo => repo.GetMessage()).Returns("This is a test message.");
            var service = new GetMessageService(mockBlobRepo.Object);
            var userName = "TestUser";

            // Act
            var result = service.GetMessage(userName);

            // Assert
            Assert.Equal("Welcome to Calderilla Service TestUser! This is a test message.", result);
        }
    }
}
