
using Calderilla.DataAccess;
using Calderilla.Test.Utils;
using Moq;

namespace Calderilla.Services.Tests
{
    public class OperationsServiceShould
    {
        [Fact]
        public void GetOperations_ReturnsNonNullResult_WhenCalled()
        {
            // Arrange
            var mockBlobRepo = new Mock<IBlobRepo>();
            var fakeOperations = FakeOperationGenerator.GetFakeOperations(5);
            mockBlobRepo.Setup(repo => repo.GetOperations(It.IsAny<int>(), It.IsAny<int>())).Returns(fakeOperations);
            var service = new OperationsService(mockBlobRepo.Object);

            // Act
            var result = service.GetOperations(1, 2025);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fakeOperations.Count, result.Count);
        }
    }
}
