using Calderilla.DataAccess;
using Calderilla.Services.Accounts;
using Moq;

namespace Calderilla.Services.Tests.Accounts;

public class AccountsServiceTests_GetYearlySummaryAsync
{

    private readonly Mock<IOperationsRepository> _mockRepository;
    private readonly AccountsService _service;

    public AccountsServiceTests_GetYearlySummaryAsync()
    {
        _mockRepository = new Mock<IOperationsRepository>();
        _service = new AccountsService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetYearlySummaryAsync_ReturnsCorrectSummary_ForEachMonth()
    {
        // Arrange
        var userId = "user1";
        var currentAccount = Guid.NewGuid();
        var year = 2025;
        var allFakeOperations = new List<Calderilla.Domain.Operation>();
        for (int month = 1; month <= 12; month++)
        {
            var fakeOps = Calderilla.Test.Utils.FakeOperationGenerator.GetFakeOperations(2, month, year);
            // Ensure one income and one expense per month
            fakeOps[0].Amount = 100 * month; // income
            fakeOps[0].Ignore = false;
            fakeOps[0].OperationDate = new DateTime(year, month, 1);
            fakeOps[0].Balance = 1000 + month;
            fakeOps[0].MonthOperationNumber = 2;
            fakeOps[1].Amount = -50 * month; // expense
            fakeOps[1].Ignore = false;
            fakeOps[1].OperationDate = new DateTime(year, month, 2);
            fakeOps[1].Balance = 900 + month;
            fakeOps[1].MonthOperationNumber = 1;
            allFakeOperations.AddRange(fakeOps);
        }
        _mockRepository.Setup(r => r.GetOperationsAsync(userId, currentAccount, year)).ReturnsAsync(allFakeOperations);

        // Act
        var result = await _service.GetYearlySummaryAsync(userId, currentAccount, year);

        // Assert
        Assert.Equal(12, result.Count);
        for (int month = 1; month <= 12; month++)
        {
            var summary = result[month - 1];
            Assert.Equal(month, summary.Month);
            Assert.Equal(100 * month, summary.Incomes);
            Assert.Equal(50 * month, summary.Expenses);
            Assert.Equal(50 * month, summary.Result);
            Assert.Equal(900 + month, summary.MonthEndBalance);
        }
    }

    [Fact]
    public async Task GetYearlySummaryAsync_ReturnsZeroes_WhenNoOperations()
    {
        // Arrange
        var userId = "user1";
        var currentAccount = Guid.NewGuid();
        var year = 2025;
        _mockRepository.Setup(r => r.GetOperationsAsync(userId, currentAccount, year)).ReturnsAsync(new List<Calderilla.Domain.Operation>());

        // Act
        var result = await _service.GetYearlySummaryAsync(userId, currentAccount, year);

        // Assert
        Assert.Equal(12, result.Count);
        foreach (var summary in result)
        {
            Assert.Equal(0, summary.Incomes);
            Assert.Equal(0, summary.Expenses);
            Assert.Equal(0, summary.Result);
            Assert.Equal(0, summary.MonthEndBalance);
        }
    }
}
