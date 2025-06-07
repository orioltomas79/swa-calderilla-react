using System;

namespace Calderilla.Services.Accounts;

public class MonthSummary
{
    public int Month { get; set; }
    public decimal Incomes { get; set; }
    public decimal Expenses { get; set; }
    public decimal Result { get; set; }
    public decimal MonthEndBalance { get; set; }
}
