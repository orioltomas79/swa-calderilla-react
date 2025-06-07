using Calderilla.Services.Accounts;

namespace Calderilla.Api.Functions.CurrentAccount;

public class GetCurrentAccountYearlySummaryResponse
{
    public List<MonthSummary> Months { get; set; } = [];
}
