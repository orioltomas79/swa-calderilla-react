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
            var operations = CreateOperation("Coffee");
            var last12Months = CreateLast12Months([("Coffee", "Food"), ("Coffee", "Food"),]);
            MockSetupLast12Months(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.Equal("Food", operations[0].Type);
        }

        [Fact]
        public async Task EnrichOperationTypeAsync_DoesNotSetType_WhenNoMatchingDescription()
        {
            // Arrange
            var operations = CreateOperation("Book");
            var last12Months = CreateLast12Months([("Coffee", "Food")]);
            MockSetupLast12Months(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.True(string.IsNullOrEmpty(operations[0].Type));
        }

        [Fact]
        public async Task EnrichOperationTypeAsync_SetsMostFrequentType_WhenMultipleTypesExist()
        {
            // Arrange
            var operations = CreateOperation("Lunch");
            var last12Months = CreateLast12Months([("Lunch", "Food"), ("Lunch", "Food"), ("Lunch", "Meal")]);
            MockSetupLast12Months(last12Months);

            // Act
            await _service.EnrichOperationTypeAsync(UserId, CurrentAccount, operations, DateTime.Now.Year, DateTime.Now.Month);

            // Assert
            Assert.Equal("Food", operations[0].Type);
        }

        private static List<Operation> CreateOperation(string description)
        {
            return
            [
                new() {
                    Id = Guid.NewGuid(),
                    Description = description,
                    Type = string.Empty,
                    MonthOperationNumber = 1,
                    OperationDate = DateTime.Now,
                    ValueDate = DateTime.Now,
                    Amount = 1,
                    Balance = 10,
                    Payer = string.Empty,
                    Ignore = false,
                    Reviewed = false },
            ];
        }

        private static List<Operation> CreateLast12Months(List<(string Description, string? Type)> data)
        {
            var list = new List<Operation>();
            int opNum = 2;
            foreach (var item in data)
            {
                list.Add(new Operation
                {
                    Id = Guid.NewGuid(),
                    Description = item.Description,
                    Type = item.Type,
                    MonthOperationNumber = opNum++,
                    OperationDate = DateTime.Now.AddMonths(-1),
                    ValueDate = DateTime.Now.AddMonths(-1),
                    Amount = 1,
                    Balance = 1,
                    Payer = string.Empty,
                    Ignore = false,
                    Reviewed = false
                });
            }
            return list;
        }

        private void MockSetupLast12Months(List<Operation> last12Months)
        {
            _mockRepository.Setup(r => r.GetOperationsLast12MonthsAsync(UserId, CurrentAccount, DateTime.Now.Year, DateTime.Now.Month)).ReturnsAsync(last12Months);
        }
    }
}
