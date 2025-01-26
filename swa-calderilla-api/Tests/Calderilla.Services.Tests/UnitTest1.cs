using Calderilla.DataAccess;
using Moq;

namespace Calderilla.Services.Tests
{
    public class InitTest1
    {
        [Fact]
        public void GetMessage_ReturnsCorrectMessage()
        {
            // Arrange
            var mockBlobRepo = new Mock<IBlobRepo>();
            mockBlobRepo.Setup(repo => repo.GetMessage()).Returns("This is a test message.");
            var service = new Service1(mockBlobRepo.Object);
            var userName = "TestUser";

            // Act
            var result = service.GetMessage(userName);

            // Assert
            Assert.Equal("Welcome to Calderilla Service TestUser! This is a test message.", result);
        }
    }
}
