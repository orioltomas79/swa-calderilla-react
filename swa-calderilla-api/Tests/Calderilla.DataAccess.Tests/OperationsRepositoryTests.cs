using Bogus.DataSets;
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


        [Fact]
        public async Task GetOperationsLast12MonthsAsync_ShouldReturnAggregatedOperations()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var now = DateTime.Now;
            var allFakeOperations = new List<Operation>();

            for (int i = 1; i <= 12; i++)
            {
                var date = now.AddMonths(-i);
                var year = date.Year;
                var month = date.Month;
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                var fakeOps = FakeOperationGenerator.GetFakeOperations(2, month, year);
                allFakeOperations.AddRange(fakeOps);
                _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync(fakeOps);
            }

            // Act
            var result = await _operationsRepository.GetOperationsLast12MonthsAsync(userId, currentAccount, now.Year, now.Month);

            // Assert
            for (int i = 1; i <= 12; i++)
            {
                var date = now.AddMonths(-i);
                var year = date.Year;
                var month = date.Month;
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                _mockBlobRepository.Verify(repo => repo.ReadListAsync<Operation>(blobName), Times.Once);
            }
            Assert.Equal(24, result.Count()); // 2 ops per month * 12 months
            Assert.All(allFakeOperations, op => Assert.Contains(op, result));
        }

        [Fact]
        public async Task GetOperationsLast12MonthsAsync_ShouldReturnEmpty_WhenNoOperations()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var now = new DateTime(2025, 5, 24);
            for (int i = 1; i <= 12; i++)
            {
                var date = now.AddMonths(-i);
                var year = date.Year;
                var month = date.Month;
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync([]);
            }

            // Act
            var result = await _operationsRepository.GetOperationsLast12MonthsAsync(userId, currentAccount, now.Year, now.Month);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetOperationsAsync_Year_ShouldReturnAggregatedOperations()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var year = 2025;
            var allFakeOperations = new List<Operation>();
            for (int month = 1; month <= 12; month++)
            {
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                var fakeOps = FakeOperationGenerator.GetFakeOperations(2, month, year);
                allFakeOperations.AddRange(fakeOps);
                _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync(fakeOps);
            }

            // Act
            var result = await _operationsRepository.GetOperationsAsync(userId, currentAccount, year);

            // Assert
            for (int month = 1; month <= 12; month++)
            {
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                _mockBlobRepository.Verify(repo => repo.ReadListAsync<Operation>(blobName), Times.Once);
            }
            Assert.Equal(24, result.Count()); // 2 ops per month * 12 months
            Assert.All(allFakeOperations, op => Assert.Contains(op, result));
        }

        [Fact]
        public async Task GetOperationsAsync_Year_ShouldReturnEmpty_WhenNoOperations()
        {
            // Arrange
            var currentAccount = Guid.NewGuid();
            var year = 2025;
            for (int month = 1; month <= 12; month++)
            {
                var blobName = $"{userId}/{currentAccount}/{year}/{month}.json";
                _mockBlobRepository.Setup(repo => repo.ReadListAsync<Operation>(blobName)).ReturnsAsync([]);
            }

            // Act
            var result = await _operationsRepository.GetOperationsAsync(userId, currentAccount, year);

            // Assert
            Assert.Empty(result);
        }
    }
}
