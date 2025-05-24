using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.ErrorHandling;
using Calderilla.Api.Functions.Dev;
using Calderilla.Services.Banks.Sabadell;
using Calderilla.Services.Operations;
using Calderilla.Api.Auth;

namespace Calderilla.Api.Functions.Banks.Sabadell
{
    public class UploadSabadellExtractFunction(
        ILogger<UploadSabadellExtractFunction> logger,
        ISabadellService sabadellService,
        IOperationsService operationsService)
    {
        private readonly ILogger<UploadSabadellExtractFunction> _logger = logger;
        private readonly ISabadellService _sabadellService = sabadellService;
        private readonly IOperationsService _operationsService = operationsService;

        [Function(nameof(UploadSabadellExtractAsync))]
        [OpenApiOperation(operationId: nameof(UploadSabadellExtractAsync), tags: [ApiEndpoints.SabadellEndpointsTag], Summary = "Uploads a document")]
        [OpenApiParameter(name: "currentAccount", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
        [OpenApiParameter(name: "month", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The month of the operations")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadSabadellExtractRequest), Required = true, Description = "The document file to upload.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(UploadSabadellExtractResponse), Description = "Returns the details of the uploaded document")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> UploadSabadellExtractAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ApiEndpoints.UploadSabadellBankExtract)] HttpRequest req, Guid currentAccount, int year, int month)
        {
            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            // Get the file from the request
            _logger.LogDebug("Getting Sabadell file from request");
            var form = await req.ReadFormAsync();
            var requestDocumentName = nameof(UploadSabadellExtractRequest.Document);
            var file = form.Files.GetFile(requestDocumentName);
            if (file == null)
            {
                var errors = new Dictionary<string, string[]> {
                    { "file", new[] { $"No file named {requestDocumentName} was provided." } }
                };
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);
            }

            // Get the file content
            _logger.LogDebug("Reading Sabadell file");
            string fileContent;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                fileContent = await reader.ReadToEndAsync();
            }

            // Extract the data from the file
            _logger.LogDebug("Extracting data from Sabadell file");
            var result = _sabadellService.GetBankExtractData(fileContent, month, year);

            // Enrich the data
            _logger.LogDebug("Enriching operation type");
            await _operationsService.EnrichOperationTypeAsync(claimsPrincipal.GetName(), currentAccount, result.Operations, year, month).ConfigureAwait(false);

            // Save the data
            _logger.LogDebug("Saving operations");
            await _operationsService.SaveOperationAsync(result.Operations, claimsPrincipal.GetName(), currentAccount, year, month).ConfigureAwait(false);

            // Return the result
            _logger.LogDebug("Returning Sabadell file result");
            var response = new UploadSabadellExtractResponse()
            {
                SabadellExtractPipe = result.RawData,
                Operations = result.Operations
            };

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        public class UploadSabadellExtractRequest
        {
            public required byte[] Document { get; set; }
        }

        public class UploadSabadellExtractResponse
        {
            public required List<string> SabadellExtractPipe { get; set; }

            public required IEnumerable<Domain.Operation> Operations { get; set; }
        }
    }
}
