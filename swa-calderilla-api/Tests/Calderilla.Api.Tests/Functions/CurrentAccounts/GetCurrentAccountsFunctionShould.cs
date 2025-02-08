using Calderilla.Api.Functions.CurrentAccount;
using Calderilla.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Operations
{
    public class GetCurrentAccountsFunctionShould
    {
        [Fact]
        public void GetCurrentAccountsFunctionShouldReturnCurrentAccounts()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetCurrentAccountFunction>>();

            var function = new GetCurrentAccountFunction(loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";

            // Act
            var result = function.GetCurrentAccounts(request);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var operations = Assert.IsType<List<CurrentAccount>>(objectResult.Value);
            Assert.True(operations.Count > 0);
        }
    }
}
