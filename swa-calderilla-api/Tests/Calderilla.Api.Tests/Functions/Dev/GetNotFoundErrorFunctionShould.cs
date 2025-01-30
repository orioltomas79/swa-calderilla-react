using System.Text;
using Calderilla.Api.Functions.Dev;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Dev
{
    public class GetNotFoundErrorFunctionShould
    {
        [Fact]
        public void GetNotFoundErrorFunctionShouldReturn404()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetNotFoundErrorFunction>>();
            var function = new GetNotFoundErrorFunction(loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

            // Act
            var result = function.GetNotFoundError(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status404NotFound);
        }
    }
}
