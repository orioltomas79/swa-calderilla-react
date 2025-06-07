using System;

namespace Calderilla.Services.Accounts;

public interface IAccountsService
{
    Task<List<MonthSummary>> GetYearlySummaryAsync(string userId, Guid currentAccount, int year);
}
