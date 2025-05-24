using Calderilla.Api.Functions.Operation;
using Calderilla.Domain;
using Calderilla.Services.Operations;
using Calderilla.Test.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Operations
{
    public class GetOperationsFunctionShould
    {
        private readonly Mock<ILogger<GetOperationsFunction>> _loggerMock;
        private readonly Mock<IOperationsService> _operationsServiceMock;
        private readonly GetOperationsFunction _function;

        public GetOperationsFunctionShould()
        {
            _loggerMock = new Mock<ILogger<GetOperationsFunction>>();
            _operationsServiceMock = new Mock<IOperationsService>();
            _function = new GetOperationsFunction(_loggerMock.Object, _operationsServiceMock.Object);
        }

        [Fact]
        public async Task GetOperationsFunctionShouldReturnOperationsAsync()
        {
            // Arrange
            var fakeOperations = FakeOperationGenerator.GetFakeOperations(5);
            _operationsServiceMock.Setup(service => service.GetOperationsAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);
            var request = CreateHttpRequest();

            // Act
            var result = await _function.GetOperationsAsync(request, Guid.NewGuid(), 2025, 1);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var operations = Assert.IsType<List<Operation>>(objectResult.Value);
            Assert.Equal(fakeOperations.Count, operations.Count);
        }

        [Fact]
        public async Task GetOperationsFunctionShouldReturnValidationErrorsWhenInvalidParametersAsync()
        {
            // Arrange
            var fakeOperations = FakeOperationGenerator.GetFakeOperations(5);
            _operationsServiceMock.Setup(service => service.GetOperationsAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);
            var request = CreateHttpRequest();

            // Act
            var result = await _function.GetOperationsAsync(request, Guid.NewGuid(), 0, 0);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var operations = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            Assert.Equal(objectResult.StatusCode, StatusCodes.Status400BadRequest);
            Assert.Equal(2, operations.Errors.Count);
        }

        private static HttpRequest CreateHttpRequest()
        {
            var httpContext = new DefaultHttpContext();
            return httpContext.Request;
        }
    }
}
