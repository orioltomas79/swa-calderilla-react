using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.ErrorHandling;
using Calderilla.DataAccess;
using Calderilla.Api.Functions.Dev;
using Calderilla.Services.Operations;
using Calderilla.Services.Ing;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace Calderilla.Api.Functions.Ing
{
    public class UploadIngExtractFunction
    {
        private readonly ILogger<UploadIngExtractFunction> _logger;
        private readonly IIngService _ingService;

        public UploadIngExtractFunction(ILogger<UploadIngExtractFunction> logger, IIngService ingService)
        {
            _logger = logger;
            _ingService = ingService;
        }

        [Function(nameof(UploadDocumentAsync))]
        [OpenApiOperation(operationId: nameof(UploadDocumentAsync), tags: [ApiEndpoints.IngEndpointsTag], Summary = "Uploads a document")]
        [OpenApiParameter(name: "currentAccount", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
        [OpenApiParameter(name: "month", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The month of the operations")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadIngExtractRequest), Required = true, Description = "The document file to upload.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(UploadIngExtractResponse), Description = "Returns the details of the uploaded document")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> UploadDocumentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ApiEndpoints.UploadIngBankExtract)] HttpRequest req, Guid currentAccount, int year, int month)
        {
            // 1. Parse the multipart form
            var form = await req.ReadFormAsync();

            // 2. Grab the file and its original name
            var requestDocumentName = nameof(UploadIngExtractRequest.Document);
            var file = form.Files.GetFile(requestDocumentName);
            if (file == null)
            {
                var errors = new Dictionary<string, string[]> {
                    { "file", new[] { $"No file named {requestDocumentName} was provided." } }
                };
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);
            }

            // 3. Read the file into a memory stream
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            ms.Position = 0; 
            var workbook = new HSSFWorkbook(ms);

            // 4. Call the service to get the data
            var result = _ingService.GetBankExtractData(workbook, month, year);

            var response = new UploadIngExtractResponse()
            {
                IngExtractCsv = result.CsvData,
                Operations = result.Operations
            };

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        public class UploadIngExtractRequest
        {
            public required byte[] Document { get; set; }
        }

        public class UploadIngExtractResponse
        {
            public required string IngExtractCsv { get; set; }

            public required IEnumerable<Domain.Operation> Operations { get; set; }
        }
    }
}
