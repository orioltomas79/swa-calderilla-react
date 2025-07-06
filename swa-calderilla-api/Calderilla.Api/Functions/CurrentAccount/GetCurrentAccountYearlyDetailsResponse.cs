using Calderilla.Services.Accounts;

namespace Calderilla.Api.Functions.CurrentAccount;

public class GetCurrentAccountYearlyDetailsResponse
{
    public List<OperationTypeSummary> Types { get; set; } = [];
}
