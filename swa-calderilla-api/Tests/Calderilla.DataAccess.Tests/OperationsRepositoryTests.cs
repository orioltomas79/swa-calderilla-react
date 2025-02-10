using Calderilla.Domain;
using Calderilla.Test.Utils;
using Moq;

namespace Calderilla.DataAccess.Tests
{
    public class OperationsRepositoryTests
    {
        private const string userId = "user1";

        private readonly Mock<IBlobRepository> _mockBlobRepository;
        private readonly OperationsRepository _operationsRepository;

        public OperationsRepositoryTests()
        {
            _mockBlobRepository = new Mock<IBlobRepository>();
            _operationsRepository = new OperationsRepository(_mockBlobRepository.Object);
        }

        [Fact]
        public async Task GetOperationsAsync_ShouldReturnOperations()
        {
            // Arrange
            var operations = FakeOperationGenerator.GetFakeOperations(1);
            var currentAccount = Guid.NewGuid();
            var year = operations.First().OperationDate.Year;
            var month = operations.First().OperationDate.Month;
            var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";

            _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync(operations);

            // Act
            var result = await _operationsRepository.GetOperationsAsync(userId, currentAccount, year, month);

            // Assert
            _mockBlobRepository.Verify(repo => repo.ReadListAsync<Operation>(blobName), Times.Once);
            Assert.Equal(operations, result);
        }
    }
}
