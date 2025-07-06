using Calderilla.DataAccess;
using Calderilla.Domain;

namespace Calderilla.Services.Accounts;

public class AccountsService(IOperationsRepository operationsRepository) : IAccountsService
{
    private readonly IOperationsRepository _operationsRepository = operationsRepository ?? throw new ArgumentNullException(nameof(operationsRepository));

    public async Task<List<MonthSummary>> GetYearlySummaryAsync(string userId, Guid currentAccount, int year)
    {
        var operations = (await _operationsRepository.GetOperationsAsync(userId, currentAccount, year))?.ToList() ?? new List<Operation>();

        var summaries = new List<MonthSummary>();

        for (int month = 1; month <= 12; month++)
        {
            var monthOps = operations
                .Where(op => op.OperationDate.Month == month && op.OperationDate.Year == year && !op.Ignore)
                .ToList();

            decimal incomes = monthOps.Where(op => op.Amount > 0).Sum(op => op.Amount);
            decimal expenses = monthOps.Where(op => op.Amount < 0).Sum(op => Math.Abs(op.Amount));
            decimal result = incomes - expenses;
            decimal monthEndBalance = monthOps.OrderBy(op => op.MonthOperationNumber).FirstOrDefault()?.Balance ?? 0m;

            summaries.Add(new MonthSummary
            {
                Month = month,
                Incomes = incomes,
                Expenses = expenses,
                Result = result,
                MonthEndBalance = monthEndBalance
            });
        }

        return summaries;
    }

    public async Task<List<OperationTypeSummary>> GetYearlyTypeSummaryAsync(string userId, Guid currentAccount, int year)
    {
        var operations = (await _operationsRepository.GetOperationsAsync(userId, currentAccount, year))?.ToList() ?? new List<Operation>();

        var grouped = operations
            .Where(op => op.OperationDate.Year == year && !op.Ignore)
            .GroupBy(op => op.Type ?? "Unknown");

        var result = new List<OperationTypeSummary>();

        foreach (var group in grouped)
        {
            var summary = new OperationTypeSummary
            {
                Type = group.Key,
                Jan = group.Where(op => op.OperationDate.Month == 1).Sum(op => op.Amount),
                Feb = group.Where(op => op.OperationDate.Month == 2).Sum(op => op.Amount),
                Mar = group.Where(op => op.OperationDate.Month == 3).Sum(op => op.Amount),
                Apr = group.Where(op => op.OperationDate.Month == 4).Sum(op => op.Amount),
                May = group.Where(op => op.OperationDate.Month == 5).Sum(op => op.Amount),
                Jun = group.Where(op => op.OperationDate.Month == 6).Sum(op => op.Amount),
                Jul = group.Where(op => op.OperationDate.Month == 7).Sum(op => op.Amount),
                Aug = group.Where(op => op.OperationDate.Month == 8).Sum(op => op.Amount),
                Sep = group.Where(op => op.OperationDate.Month == 9).Sum(op => op.Amount),
                Oct = group.Where(op => op.OperationDate.Month == 10).Sum(op => op.Amount),
                Nov = group.Where(op => op.OperationDate.Month == 11).Sum(op => op.Amount),
                Dec = group.Where(op => op.OperationDate.Month == 12).Sum(op => op.Amount),
            };
            summary.Total = summary.Jan + summary.Feb + summary.Mar + summary.Apr + summary.May + summary.Jun +
                            summary.Jul + summary.Aug + summary.Sep + summary.Oct + summary.Nov + summary.Dec;
            result.Add(summary);
        }

        return [.. result.OrderBy(s => s.Total)];
    }
}
