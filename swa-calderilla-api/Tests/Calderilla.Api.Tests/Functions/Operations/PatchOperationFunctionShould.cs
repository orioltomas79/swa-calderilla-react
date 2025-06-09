using System.Text;
using Calderilla.Api.Functions.Operation;
using Calderilla.Domain;
using Calderilla.Services.Operations;
using Calderilla.Test.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Calderilla.Api.Tests.Functions.Operations;

public class PatchOperationFunctionShould
{
    private readonly Mock<ILogger<PatchOperationFunction>> _loggerMock;
    private readonly Mock<IOperationsService> _operationsServiceMock;
    private readonly PatchOperationFunction _function;

    public PatchOperationFunctionShould()
    {
        _loggerMock = new Mock<ILogger<PatchOperationFunction>>();
        _operationsServiceMock = new Mock<IOperationsService>();
        _function = new PatchOperationFunction(_loggerMock.Object, _operationsServiceMock.Object);
    }

    [Fact]
    public async Task PatchOperation_ShouldUpdateTypeAndReturnOk()
    {
        // Arrange
        var fakeOperations = FakeOperationGenerator.GetFakeOperations(2);
        var operationToPatch = fakeOperations[0];
        var newType = "UpdatedType";
        var requestBody = System.Text.Json.JsonSerializer.Serialize(new PatchOperationRequest { Type = newType });
        var request = CreateHttpRequestWithBody(requestBody);
        _operationsServiceMock.Setup(s => s.GetOperationsAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);
        _operationsServiceMock.Setup(s => s.SaveOperationAsync(It.IsAny<IEnumerable<Operation>>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        var result = await _function.PatchOperationAsync(request, Guid.NewGuid(), 2025, 6, operationToPatch.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedOperation = Assert.IsType<Operation>(okResult.Value);
        Assert.Equal(newType, updatedOperation.Type);
    }

    [Fact]
    public async Task PatchOperation_ShouldReturnValidationErrors_WhenInvalidParameters()
    {
        // Arrange
        var request = CreateHttpRequestWithBody("{ } ");
        // Act
        var result = await _function.PatchOperationAsync(request, Guid.NewGuid(), 0, 0, Guid.NewGuid());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var details = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.True(details.Errors.ContainsKey("month"));
        Assert.True(details.Errors.ContainsKey("year"));
    }

    [Fact]
    public async Task PatchOperation_ShouldReturnNotFound_WhenOperationDoesNotExist()
    {
        // Arrange
        var fakeOperations = FakeOperationGenerator.GetFakeOperations(2);
        var request = CreateHttpRequestWithBody("{ } ");
        _operationsServiceMock.Setup(s => s.GetOperationsAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(fakeOperations);

        // Act
        var result = await _function.PatchOperationAsync(request, Guid.NewGuid(), 2025, 6, Guid.NewGuid());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        var details = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal("Operation not found", details.Title);
    }

    [Fact]
    public async Task PatchOperation_ShouldReturnValidationError_WhenBodyIsInvalid()
    {
        // Arrange
        var request = CreateHttpRequestWithBody("not a json");

        // Act
        var result = await _function.PatchOperationAsync(request, Guid.NewGuid(), 2025, 6, Guid.NewGuid());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var details = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        Assert.True(details.Errors.ContainsKey("body"));
    }

    private static HttpRequest CreateHttpRequestWithBody(string body)
    {
        var context = new DefaultHttpContext();
        var request = context.Request;
        var bytes = Encoding.UTF8.GetBytes(body);
        request.Body = new MemoryStream(bytes);
        request.ContentLength = bytes.Length;
        request.ContentType = "application/json";
        return request;
    }
}
