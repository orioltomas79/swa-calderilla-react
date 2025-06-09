using Calderilla.Api.Functions.Dev;
using System.Net;
using System.Text.Json;
using Calderilla.Api.Auth;
using Calderilla.Api.ErrorHandling;
using Calderilla.Services.Operations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Calderilla.Api.Functions.Operation
{
    public class PatchOperationFunction(ILogger<PatchOperationFunction> logger, IOperationsService operationsService)
    {
        private readonly ILogger<PatchOperationFunction> _logger = logger;
        private readonly IOperationsService _operationsService = operationsService;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [Function(nameof(PatchOperationAsync))]
        [OpenApiOperation(operationId: nameof(PatchOperationAsync), tags: [ApiEndpoints.OperationsEndpointsTag], Summary = "Patch an operation")]
        [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operation")]
        [OpenApiParameter(name: "month", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The month of the operation")]
        [OpenApiParameter(name: "operationId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The operation ID")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(PatchOperationRequest), Required = true, Description = "The new values")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Domain.Operation), Description = "Returns the updated operation")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Validation error")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Operation not found")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Server error")]
        public async Task<IActionResult> PatchOperationAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = ApiEndpoints.PatchOperation)] HttpRequest req,
            Guid accountId,
            int year,
            int month,
            Guid operationId)
        {
            var errors = new Dictionary<string, string[]>();
            if (month is < 1 or > 12)
                errors.Add("month", ["The month must be between 1 and 12."]);
            if (year is < 2010 or > 3000)
                errors.Add("year", ["The year must be between 2010 and 3000."]);
            if (errors.Count > 0)
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);

            PatchOperationRequest? patchRequest;
            try
            {
                patchRequest = await JsonSerializer.DeserializeAsync<PatchOperationRequest>(req.Body, _jsonSerializerOptions);
                if (patchRequest == null)
                    return ValidationProblemDetailsHelper.ValidationProblemDetails(req, new Dictionary<string, string[]> { { "body", ["Invalid or missing request body."] } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize patch request body");
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, new Dictionary<string, string[]> { { "body", ["Invalid request body format."] } });
            }

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);
            var userId = claimsPrincipal.GetName();
            var operations = (await _operationsService.GetOperationsAsync(userId, accountId, year, month)).ToList();
            var operation = operations.FirstOrDefault(o => o.Id == operationId);
            if (operation == null)
            {
                return ProblemDetailsHelper.NotFoundProblemDetails(req, "Operation not found", $"No operation with ID {operationId} found for the specified account/year/month.");
            }

            var updated = false;

            if (patchRequest.Type != null)
            {
                operation.Type = patchRequest.Type;
                updated = true;
            }

            if (patchRequest.Ignore != null)
            {
                operation.Ignore = patchRequest.Ignore.Value;
                updated = true;
            }

            if (updated)
            {
                await _operationsService.SaveOperationAsync(operations, userId, accountId, year, month);
            }

            return new OkObjectResult(operation);
        }
    }
}
