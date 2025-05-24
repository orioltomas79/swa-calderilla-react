using Calderilla.DataAccess;
using Calderilla.Domain;
using Calderilla.Services.Operations;
using Moq;

namespace Calderilla.Services.Tests.Operations
{
    public class OperationsServiceTests_EnrichOperationTypeAsync
    {
        private readonly Mock<IOperationsRepository> _mockRepository;
        private readonly OperationsService _service;

        public OperationsServiceTests_EnrichOperationTypeAsync()
        {
            _mockRepository = new Mock<IOperationsRepository>();
            _service = new OperationsService(_mockRepository.Object);
        }

        private readonly string UserId = "test";
        private readonly Guid CurrentAccount = Guid.NewGuid();

        [Fact]
        public async Task EnrichOperationTypeAsync_SetsType_WhenMatchingDescriptionExists()
        {
            // Arrange
            var operations = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Coffee", Type = null, MonthOperationNumber = 1, OperationDate = DateTime.Now, ValueDate = DateTime.Now, Amount = 2, Balance = 10, Payer = "A", Ignore = false, Reviewed = false },
            };
            var last12Months = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Coffee", Type = "Food", MonthOperationNumber = 2, OperationDate = DateTime.Now.AddMonths(-1), ValueDate = DateTime.Now.AddMonths(-1), Amount = 2, Balance = 8, Payer = "A", Ignore = false, Reviewed = false },
                new() { Id = Guid.NewGuid(), Description = "Coffee", Type = "Food", MonthOperationNumber = 3, OperationDate = DateTime.Now.AddMonths(-2), ValueDate = DateTime.Now.AddMonths(-2), Amount = 2, Balance = 6, Payer = "A", Ignore = false, Reviewed = false },
            };
            _mockRepository.Setup(r => r.GetOperationsLast12MonthsAsync(UserId, CurrentAccount, DateTime.Now.Year, DateTime.Now.Month)).ReturnsAsync(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.Equal("Food", operations[0].Type);
        }

        [Fact]
        public async Task EnrichOperationTypeAsync_DoesNotSetType_WhenNoMatchingDescription()
        {
            // Arrange
            var operations = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Book", Type = null, MonthOperationNumber = 1, OperationDate = DateTime.Now, ValueDate = DateTime.Now, Amount = 10, Balance = 20, Payer = "B", Ignore = false, Reviewed = false },
            };
            var last12Months = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Coffee", Type = "Food", MonthOperationNumber = 2, OperationDate = DateTime.Now.AddMonths(-1), ValueDate = DateTime.Now.AddMonths(-1), Amount = 2, Balance = 8, Payer = "A", Ignore = false, Reviewed = false },
            };
            _mockRepository.Setup(r => r.GetOperationsLast12MonthsAsync(UserId, CurrentAccount, DateTime.Now.Year, DateTime.Now.Month)).ReturnsAsync(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.Null(operations[0].Type);
        }

        [Fact]
        public async Task EnrichOperationTypeAsync_SetsMostFrequentType_WhenMultipleTypesExist()
        {
            // Arrange
            var operations = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Lunch", Type = null, MonthOperationNumber = 1, OperationDate = DateTime.Now, ValueDate = DateTime.Now, Amount = 15, Balance = 30, Payer = "C", Ignore = false, Reviewed = false },
            };
            var last12Months = new List<Operation>
            {
                new() { Id = Guid.NewGuid(), Description = "Lunch", Type = "Food", MonthOperationNumber = 2, OperationDate = DateTime.Now.AddMonths(-1), ValueDate = DateTime.Now.AddMonths(-1), Amount = 15, Balance = 15, Payer = "C", Ignore = false, Reviewed = false },
                new() { Id = Guid.NewGuid(), Description = "Lunch", Type = "Food", MonthOperationNumber = 3, OperationDate = DateTime.Now.AddMonths(-2), ValueDate = DateTime.Now.AddMonths(-2), Amount = 15, Balance = 0, Payer = "C", Ignore = false, Reviewed = false },
                new() { Id = Guid.NewGuid(), Description = "Lunch", Type = "Meal", MonthOperationNumber = 4, OperationDate = DateTime.Now.AddMonths(-3), ValueDate = DateTime.Now.AddMonths(-3), Amount = 15, Balance = -15, Payer = "C", Ignore = false, Reviewed = false },
            };
            _mockRepository.Setup(r => r.GetOperationsLast12MonthsAsync(UserId, CurrentAccount, DateTime.Now.Year, DateTime.Now.Month)).ReturnsAsync(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.Equal("Food", operations[0].Type);
        }
    }
}
