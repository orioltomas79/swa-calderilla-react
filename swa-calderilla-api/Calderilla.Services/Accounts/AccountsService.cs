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
}
