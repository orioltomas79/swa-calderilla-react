using Calderilla.Api.Functions.Operation;
using Calderilla.Domain;
using Calderilla.Services;
using Calderilla.Test.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Operations
{
    public class GetOperationsFunctionShould
    {
        [Fact]
        public async Task GetOperationsFunctionShouldReturnOperationsAsync()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOperationsFunction>>();

            var serviceMock = new Mock<IOperationsService>();
            var fakeOperations = FakeOperationGenerator.GetFakeOperations(5);
            serviceMock.Setup(service => service.GetOperationsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);

            var function = new GetOperationsFunction(loggerMock.Object, serviceMock.Object);

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";

            // Act
            var result = await function.GetOperationsAsync(request, 2025, 1);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var operations = Assert.IsType<List<Operation>>(objectResult.Value);
            Assert.Equal(fakeOperations.Count, operations.Count);
        }

        [Fact]
        public async Task GetOperationsFunctionShouldReturnValidationErrorsWhenInvalidParametersAsync()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GetOperationsFunction>>();

            var serviceMock = new Mock<IOperationsService>();
            var fakeOperations = FakeOperationGenerator.GetFakeOperations(5);
            serviceMock.Setup(service => service.GetOperationsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);

            var function = new GetOperationsFunction(loggerMock.Object, serviceMock.Object);

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = "GET";

            // Act
            var result = await function.GetOperationsAsync(request, 0, 0);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var operations = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.Equal(2, operations.Errors.Count);
        }
    }
}
