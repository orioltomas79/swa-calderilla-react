namespace Calderilla.Services.Accounts;

public interface IAccountsService
{
    Task<List<MonthSummary>> GetYearlySummaryAsync(string userId, Guid currentAccount, int year);
    Task<List<OperationTypeSummary>> GetYearlyTypeSummaryAsync(string userId, Guid currentAccount, int year);
}
