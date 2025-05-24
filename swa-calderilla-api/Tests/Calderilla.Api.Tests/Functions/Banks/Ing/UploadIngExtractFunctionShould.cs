using System.Text;
using Calderilla.Api.Functions.Banks.Ing;
using Calderilla.Services.Banks;
using Calderilla.Services.Banks.Ing;
using Calderilla.Services.Operations;
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
        private readonly Mock<IOperationsService> _operationsServiceMock;
        private readonly UploadIngExtractFunction _function;

        public UploadIngExtractFunctionShould()
        {
            _loggerMock = new Mock<ILogger<UploadIngExtractFunction>>();
            _ingServiceMock = new Mock<IIngService>();
            _operationsServiceMock = new Mock<IOperationsService>();
            _function = new UploadIngExtractFunction(_loggerMock.Object, _ingServiceMock.Object, _operationsServiceMock.Object);
        }

        [Fact]
        public async Task ReturnCreated_WhenFileIsProcessedSuccessfully()
        {
            // Arrange
            var httpRequest = CreateHttpRequestWithFile(CreateWorkbook());

            var resultData = new GetBankExtractResult
            {
                RawData = ["raw|data"],
                Operations = FakeOperationGenerator.GetFakeOperations(5)
            };

            _ingServiceMock.Setup(s => s.GetBankExtractData(It.IsAny<HSSFWorkbook>(), 5, 2025)).Returns(resultData);

            // Act
            var result = await _function.UploadIngExtractAsync(httpRequest, Guid.NewGuid(), 2025, 5);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

            var response = Assert.IsType<UploadIngExtractFunction.UploadIngExtractResponse>(createdResult.Value);
            Assert.Equal(resultData.RawData, response.IngExtractRaw);
            Assert.Equal(resultData.Operations, response.Operations);
        }

        [Fact]
        public async Task ReturnBadRequest_WhenFileIsMissing()
        {
            // Arrange
            var httpRequest = CreateHttpRequestWithoutFile();

            // Act
            var result = await _function.UploadIngExtractAsync(httpRequest, Guid.NewGuid(), 2025, 5);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            var details = Assert.IsType<ValidationProblemDetails>(objectResult.Value);
            var firstError = details.Errors.FirstOrDefault();
            Assert.Equal("file", firstError.Key);
            Assert.Equal("No file named Document was provided.", firstError.Value.FirstOrDefault());
        }

        private static HttpRequest CreateHttpRequestWithFile(byte[] fileContent, string fileFieldName = "Document")
        {
            var context = new DefaultHttpContext();
            var formFile = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, fileFieldName, "test.xls");
            var formCollection = new FormCollection([], new FormFileCollection { formFile });
            context.Request.Form = formCollection;
            return context.Request;
        }

        private static IFormFile CreateFormFile(string content, string name)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, name, "test.txt");
        }

        private static HttpRequest CreateHttpRequestWithoutFile()
        {
            var context = new DefaultHttpContext();
            var formCollection = new FormCollection([], new FormFileCollection());
            context.Request.Form = formCollection;
            return context.Request;
        }

        private static byte[] CreateWorkbook()
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
