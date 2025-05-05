using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.Auth;
using Calderilla.Api.ErrorHandling;
using Calderilla.DataAccess;
using Calderilla.Api.Functions.Dev;

namespace Calderilla.Api.Functions.Operation
{
    public class UploadDocumentFunction
    {
        private readonly ILogger<UploadDocumentFunction> _logger;

        public UploadDocumentFunction(ILogger<UploadDocumentFunction> logger, IBlobRepository blobRepository)
        {
            _logger = logger;
        }

        [Function(nameof(UploadDocumentAsync))]
        [OpenApiOperation(operationId: nameof(UploadDocumentAsync), tags: [ApiEndpoints.OperationsEndpointsTag], Summary = "Uploads a document")]
        [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadDocumentRequest), Required = true, Description = "The document file to upload.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns the URL of the uploaded document")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> UploadDocumentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = ApiEndpoints.UploadDocument)] HttpRequest req)
        {
            // 1. Parse the multipart form
            var form = await req.ReadFormAsync();

            // 2. Grab the file and its original name
            var requestDocumentName = nameof(UploadDocumentRequest.Document);
            var file = form.Files.GetFile(requestDocumentName);
            if (file == null)
            {
                var errors = new Dictionary<string, string[]> {
                    { "file", new[] { $"No file named {requestDocumentName} was provided." } }
                };
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);
            }

            string fileName = file.FileName;

            // 3. Read the file into a byte[]
            using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);
            byte[] content = ms.ToArray();

            // ... your processing logic here ...

            return new OkObjectResult($"Received {fileName} ({content.Length} bytes)");
        }

        public class UploadDocumentRequest
        {
            public required byte[] Document { get; set; }
        }
    }
}
