using Calderilla.Api.Functions.CurrentAccount;
using Calderilla.Services.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.CurrentAccounts;

public class GetCurrentAccountYearlySummaryFunctionShould
{
    private readonly Mock<IAccountsService> _accountsServiceMock;

    private readonly Mock<ILogger<GetCurrentAccountYearlySummaryFunction>> _loggerMock;

    private readonly GetCurrentAccountYearlySummaryFunction _function;

    public GetCurrentAccountYearlySummaryFunctionShould()
    {
        _accountsServiceMock = new Mock<IAccountsService>();
        _loggerMock = new Mock<ILogger<GetCurrentAccountYearlySummaryFunction>>();
        _function = new GetCurrentAccountYearlySummaryFunction(_loggerMock.Object, _accountsServiceMock.Object);
    }

    [Fact]
    public async Task GetCurrentAccountsFunctionShouldReturnCurrentAccountsAsync()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var request = httpContext.Request;
        request.Method = "GET";

        // Act
        var result = await _function.GetCurrentAccountYearlySummary(request, Guid.NewGuid(), 2025);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GetCurrentAccountYearlySummaryResponse>(objectResult.Value);
        Assert.NotNull(response);
    }

}
