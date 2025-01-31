using System.Text;
using Calderilla.Api.Functions.Dev;
using Calderilla.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Dev
{
    public class GetMessageFunctionShould
    {
        [Fact]
        public void GetMessageFunctionShouldReturnAMessage()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetMessageFunction>>();

            var serviceMock = new Mock<IGetMessageService>();
            serviceMock.Setup(service => service.GetMessage(It.IsAny<string>())).Returns("This is a test message.");

            var function = new GetMessageFunction(loggerMock.Object, serviceMock.Object);

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";
            request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{}"));

            // Act
            var result = function.GetMessage(request);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(objectResult.Value, "This is a test message.");
        }
    }
}
