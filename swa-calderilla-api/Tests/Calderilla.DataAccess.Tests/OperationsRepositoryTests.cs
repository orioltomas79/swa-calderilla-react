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
        public async Task AddOperationAsync_ShouldAddOperation()
        {
            // Arrange
            var operation = FakeOperationGenerator.GetFakeOperations(1).First();
            var year = operation.OperationDate.Year;
            var month = operation.OperationDate.Month;
            var blobName = $"{userId}/{year}/{month}.json";

            var operations = new List<Operation>();

            _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync(operations);

            // Act
            await _operationsRepository.AddOperationAsync(userId, operation);

            // Assert
            _mockBlobRepository.Verify(repo => repo.ReadListAsync<Operation>(blobName), Times.Once);
            _mockBlobRepository.Verify(repo => repo.WriteListAsync(blobName, It.Is<List<Operation>>(ops => ops.Contains(operation))), Times.Once);
        }

        [Fact]
        public async Task GetOperationsAsync_ShouldReturnOperations()
        {
            // Arrange
            var operations = FakeOperationGenerator.GetFakeOperations(1);
            var year = operations.First().OperationDate.Year;
            var month = operations.First().OperationDate.Month;
            var blobName = $"{userId}/{year}/{month}.json";

            _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync(operations);

            // Act
            var result = await _operationsRepository.GetOperationsAsync(userId, year, month);

            // Assert
            _mockBlobRepository.Verify(repo => repo.ReadListAsync<Operation>(blobName), Times.Once);
            Assert.Equal(operations, result);
        }
    }
}
