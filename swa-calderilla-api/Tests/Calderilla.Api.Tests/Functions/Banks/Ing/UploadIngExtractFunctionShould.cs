using Calderilla.Api.Functions.Banks.Ing;
using Calderilla.Services.Banks;
using Calderilla.Services.Banks.Ing;
using Calderilla.Test.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NPOI.HSSF.UserModel;

namespace Calderilla.Api.Tests.Functions.Banks.Ing
{
    public class UploadIngExtractFunctionShould
    {
        private readonly Mock<ILogger<UploadIngExtractFunction>> _loggerMock;
        private readonly Mock<IIngService> _ingServiceMock;
        private readonly UploadIngExtractFunction _function;

        public UploadIngExtractFunctionShould()
        {
            _loggerMock = new Mock<ILogger<UploadIngExtractFunction>>();
            _ingServiceMock = new Mock<IIngService>();
            _function = new UploadIngExtractFunction(_loggerMock.Object, _ingServiceMock.Object);
        }

        [Fact]
        public async Task ReturnCreated_WhenFileIsProcessedSuccessfully()
        {
            // Arrange
            var mockHttpRequest = CreateMockHttpRequestWithFile(CreateWorkbook(), "mockfile.xls");

            var resultData = new GetBankExtractResult
            {
                RawData = "csv,data",
                Operations = FakeOperationGenerator.GetFakeOperations(5)
            };

            _ingServiceMock.Setup(s => s.GetBankExtractData(It.IsAny<HSSFWorkbook>(), 5, 2025)).Returns(resultData);

            // Act
            var result = await _function.UploadIngExtractAsync(mockHttpRequest.Object, Guid.NewGuid(), 2025, 5);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

            var response = Assert.IsType<UploadIngExtractFunction.UploadIngExtractResponse>(createdResult.Value);
            Assert.Equal(resultData.RawData, response.IngExtractCsv);
            Assert.Equal(resultData.Operations, response.Operations);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenFileIsMissing()
        {
            // Arrange
            var httpRequestMock = CreateMockHttpRequestWithEmptyForm();

            // Act
            var result = await _function.UploadIngExtractAsync(httpRequestMock.Object, Guid.NewGuid(), 2025, 5);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var details = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            var firstError = details.Errors.FirstOrDefault();
            Assert.Equal("file", firstError.Key);
            Assert.Equal("No file named Document was provided.", firstError.Value.FirstOrDefault());
        }

        private Mock<HttpRequest> CreateMockHttpRequestWithEmptyForm()
        {
            var httpRequestMock = new Mock<HttpRequest>();
            httpRequestMock
                .Setup(r => r.ReadFormAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));
            return httpRequestMock;
        }

        private Mock<HttpRequest> CreateMockHttpRequestWithFile(byte[] fileContent, string fileName)
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(fileContent));
            mockFile.Setup(f => f.FileName).Returns(fileName);
            mockFile.Setup(f => f.Length).Returns(fileContent.Length);

            var mockFormFileCollection = new Mock<IFormFileCollection>();
            mockFormFileCollection
                .Setup(f => f.GetFile(It.IsAny<string>()))
                .Returns(mockFile.Object);

            var mockFormCollection = new Mock<IFormCollection>();
            mockFormCollection
                .Setup(f => f.Files)
                .Returns(mockFormFileCollection.Object);

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest
                .Setup(r => r.ReadFormAsync(default))
                .ReturnsAsync(mockFormCollection.Object);

            return mockHttpRequest;
        }

        public byte[] CreateWorkbook()
        {
            using var memoryStream = new MemoryStream();
            var workbook = new HSSFWorkbook();

            var sheet = workbook.CreateSheet("Sheet1");
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Column1");
            headerRow.CreateCell(1).SetCellValue("Column2");

            var dataRow = sheet.CreateRow(1);
            dataRow.CreateCell(0).SetCellValue("Data1");
            dataRow.CreateCell(1).SetCellValue("Data2");

            workbook.Write(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
