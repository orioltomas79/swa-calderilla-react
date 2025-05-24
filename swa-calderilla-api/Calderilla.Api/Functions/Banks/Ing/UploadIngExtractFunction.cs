using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.ErrorHandling;
using Calderilla.Api.Functions.Dev;
using Calderilla.Services.Banks.Ing;
using NPOI.HSSF.UserModel;
using Calderilla.Services.Operations;
using Calderilla.Api.Auth;

namespace Calderilla.Api.Functions.Banks.Ing
{
    public class UploadIngExtractFunction(ILogger<UploadIngExtractFunction> logger, IIngService ingService, IOperationsService operationsService)
    {
        private readonly ILogger<UploadIngExtractFunction> _logger = logger;
        private readonly IIngService _ingService = ingService;
        private readonly IOperationsService _operationsService = operationsService;

        [Function(nameof(UploadIngExtractAsync))]
        [OpenApiOperation(operationId: nameof(UploadIngExtractAsync), tags: [ApiEndpoints.IngEndpointsTag], Summary = "Uploads a document")]
        [OpenApiParameter(name: "currentAccount", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
        [OpenApiParameter(name: "month", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The month of the operations")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadIngExtractRequest), Required = true, Description = "The document file to upload.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(UploadIngExtractResponse), Description = "Returns the details of the uploaded document")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> UploadIngExtractAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ApiEndpoints.UploadIngBankExtract)] HttpRequest req, Guid currentAccount, int year, int month)
        {
            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            // Get the file from the request
            _logger.LogDebug("Getting ING file from request");
            var form = await req.ReadFormAsync();
            var requestDocumentName = nameof(UploadIngExtractRequest.Document);
            var file = form.Files.GetFile(requestDocumentName);
            if (file == null)
            {
                var errors = new Dictionary<string, string[]> {
                    { "file", new[] { $"No file named {requestDocumentName} was provided." } }
                };
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);
            }

            // Get the workbook from the file
            _logger.LogDebug("Reading ING file");
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            ms.Position = 0;
            var workbook = new HSSFWorkbook(ms);

            // Extract the data from the workbook
            _logger.LogDebug("Extracting data from ING file");
            var result = _ingService.GetBankExtractData(workbook, month, year);

            // Enrich the data
            _logger.LogDebug("Enriching operation type");
            await _operationsService.EnrichOperationTypeAsync(claimsPrincipal.GetName(), currentAccount, result.Operations, year, month).ConfigureAwait(false);

            // Save the data
            _logger.LogDebug("Saving operations");
            await _operationsService.SaveOperationAsync(result.Operations, claimsPrincipal.GetName(), currentAccount, year, month).ConfigureAwait(false);

            // Return the result
            _logger.LogDebug("Returning ING file result");
            var response = new UploadIngExtractResponse()
            {
                IngExtractRaw = result.RawData,
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
            public required List<string> IngExtractRaw { get; set; }

            public required IEnumerable<Domain.Operation> Operations { get; set; }
        }
    }
}
