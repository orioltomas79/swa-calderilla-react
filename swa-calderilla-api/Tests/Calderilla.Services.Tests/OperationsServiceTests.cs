using Calderilla.DataAccess;
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
        public async Task GetOperationsAsync_NullUserId_ThrowsArgumentException()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var year = 2023;
            var month = 5;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.GetOperationsAsync(string.Empty, currentAccount, year, month));
        }

        [Fact]
        public async Task GetOperationsAsync_InvalidYear_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var userId = "user123";
            var month = 5;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, currentAccount, 0, month));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, currentAccount, 10000, month));
        }

        [Fact]
        public async Task GetOperationsAsync_InvalidMonth_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var userId = "user123";
            var year = 2023;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, currentAccount, year, 0));
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _service.GetOperationsAsync(userId, currentAccount, year, 13));
        }
    }
}
