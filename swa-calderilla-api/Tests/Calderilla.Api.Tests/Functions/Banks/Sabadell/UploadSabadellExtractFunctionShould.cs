using System.Text;
using Calderilla.Api.Functions.Banks.Sabadell;
using Calderilla.Services.Banks;
using Calderilla.Services.Banks.Sabadell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using static Calderilla.Api.Functions.Banks.Sabadell.UploadSabadellExtractFunction;

namespace Calderilla.Api.Tests.Functions.Banks.Sabadell;

public class UploadSabadellExtractFunctionShould
{
    private const int Year = 2024;
    private const int Month = 6;

    private readonly Mock<ILogger<UploadSabadellExtractFunction>> _loggerMock = new();
    private readonly Mock<ISabadellService> _sabadellServiceMock = new();

    [Fact]
    public async Task ReturnCreatedAndResponse_WhenFileIsValid()
    {
        // Arrange
        var function = CreateFunction();
        var fileContent = "file-content";
        var httpRequest = CreateHttpRequestWithFile(fileContent);
        var currentAccount = Guid.NewGuid();

        var operations = Test.Utils.FakeOperationGenerator.GetFakeOperations(5);

        var serviceResult = new GetBankExtractResult
        {
            RawData = ["pipe-data"],
            Operations = operations
        };
        _sabadellServiceMock.Setup(s => s.GetBankExtractData(fileContent, Month, Year)).Returns(serviceResult);

        // Act
        var result = await function.UploadSabadellExtractAsync(httpRequest, currentAccount, Year, Month);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);

        var response = Assert.IsType<UploadSabadellExtractResponse>(objectResult.Value);
        Assert.Equal(serviceResult.RawData, response.SabadellExtractPipe);
        Assert.Equal(serviceResult.Operations, response.Operations);
    }

    [Fact]
    public async Task ReturnBadRequest_WhenFileIsMissing()
    {
        // Arrange
        var function = CreateFunction();
        var httpRequest = CreateHttpRequestWithoutFile();

        // Act
        var result = await function.UploadSabadellExtractAsync(httpRequest, Guid.NewGuid(), Year, Month);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        var details = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
        var firstError = details.Errors.FirstOrDefault();
        Assert.Equal("file", firstError.Key);
        Assert.Equal("No file named Document was provided.", firstError.Value.FirstOrDefault());
    }

    private UploadSabadellExtractFunction CreateFunction() => new(_loggerMock.Object, _sabadellServiceMock.Object);

    private static IFormFile CreateFormFile(string content, string name)
    {
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        return new FormFile(stream, 0, stream.Length, name, "test.txt");
    }

    private static HttpRequest CreateHttpRequestWithFile(string fileContent, string fileFieldName = "Document")
    {
        var context = new DefaultHttpContext();
        var formFile = CreateFormFile(fileContent, fileFieldName);
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection { formFile });
        context.Request.Form = formCollection;
        return context.Request;
    }

    private static HttpRequest CreateHttpRequestWithoutFile()
    {
        var context = new DefaultHttpContext();
        var formCollection = new FormCollection([], new FormFileCollection());
        context.Request.Form = formCollection;
        return context.Request;
    }
}
