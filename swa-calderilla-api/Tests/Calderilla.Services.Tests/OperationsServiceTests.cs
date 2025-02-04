using Calderilla.DataAccess;
using Calderilla.Domain;
using Calderilla.Test.Utils;
using Moq;

namespace Calderilla.Services.Tests
{
    public class OperationsServiceTests
    {
        private readonly Mock<IOperationsRepository> _mockRepository;
        private readonly OperationsService _service;

        public OperationsServiceTests()
        {
            _mockRepository = new Mock<IOperationsRepository>();
            _service = new OperationsService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddOperationAsync_ValidInput_CallsRepository()
        {
            // Arrange
            var userId = "user123";
            var operation = FakeOperationGenerator.GetFakeOperations(1).First();

            // Act
            await _service.AddOperationAsync(userId, operation);

            // Assert
            _mockRepository.Verify(r => r.AddOperationAsync(userId, operation), Times.Once);
        }

        [Fact]
        public async Task AddOperationAsync_NullUserId_ThrowsArgumentException()
        {
            // Arrange
            var operation = FakeOperationGenerator.GetFakeOperations(1).First();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddOperationAsync(null, operation));
        }

        [Fact]
        public async Task AddOperationAsync_NullOperation_ThrowsArgumentNullException()
        {
            // Arrange
            var userId = "user123";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddOperationAsync(userId, null));
        }

        [Fact]
        public async Task GetOperationsAsync_ValidInput_CallsRepository()
        {
            // Arrange
            var userId = "user123";
            var year = 2023;
            var month = 5;
            var operations = FakeOperationGenerator.GetFakeOperations(1);
            _mockRepository.Setup(r => r.GetOperationsAsync(userId, year, month)).ReturnsAsync(operations);

            // Act
            var result = await _service.GetOperationsAsync(userId, year, month);

            // Assert
            Assert.Equal(operations, result);
            _mockRepository.Verify(r => r.GetOperationsAsync(userId, year, month), Times.Once);
        }

        [Fact]
        public async Task GetOperationsAsync_NullUserId_ThrowsArgumentException()
        {
            // Arrange
            var year = 2023;
            var month = 5;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetOperationsAsync(null, year, month));
        }

        [Fact]
        public async Task GetOperationsAsync_InvalidYear_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var userId = "user123";
            var month = 5;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, 0, month));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, 10000, month));
        }

        [Fact]
        public async Task GetOperationsAsync_InvalidMonth_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var userId = "user123";
            var year = 2023;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, year, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, year, 13));
        }
    }
}
