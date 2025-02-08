using Calderilla.Api.Functions.Dev;
using Calderilla.Services;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.ErrorHandling;

namespace Calderilla.Api.Functions.Operation
{
    public class GetOperationsFunction
    {
        private readonly ILogger<GetOperationsFunction> _logger;
        private readonly IOperationsService _operationsService;

        public GetOperationsFunction(ILogger<GetOperationsFunction> logger, IOperationsService operationsService)
        {
            _logger = logger;
            _operationsService = operationsService;
        }

        [Function(nameof(GetOperationsAsync))]
        [OpenApiOperation(operationId: nameof(GetOperationsAsync), tags: [ApiEndpoints.OperationsEndpointsTag], Summary = "Returns a list of operations")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
        [OpenApiParameter(name: "month", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The month of the operations")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Domain.Operation>), Description = "Returns a list of operations")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> GetOperationsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetOperations)] HttpRequest req, int year, int month)
        {
            var errors = new Dictionary<string, string[]>();

            if (month is < 1 or > 12)
            {
                errors.Add("month", ["The month must be between 1 and 12."]);
            }

            if (year is < 2010 or > 3000)
            {
                errors.Add("year", ["The year must be between 2010 and 3000."]);
            }

            if (errors.Count > 0)
            {
                return ValidationProblemDetailsHelper.ValidationProblemDetails(req, errors);
            }

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);
            var result = await _operationsService.GetOperationsAsync(claimsPrincipal.GetUserId(), year, month);

            return new OkObjectResult(result);
        }

        [Function(nameof(AddOperationAsync))]
        [OpenApiOperation(operationId: nameof(AddOperationAsync), tags: [ApiEndpoints.OperationsEndpointsTag], Summary = "Adds a new operation")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(Domain.Operation), Description = "The created operation")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ValidationProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> AddOperationAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.AddOperation)] HttpRequest req)
        {
            var operation = new Domain.Operation()
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                Description = "Test",
                Notes = "Test",
                Balance = 100,
                MonthOperationNumber = 1,
                Payer = "Test",
                OperationDate = DateTime.Now,
                ValueDate = DateTime.Now,
                Ignore = false,
                Reviewed = false,
                Type = "Test"
            };

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);
            await _operationsService.AddOperationAsync(claimsPrincipal.GetUserId(), operation);

            return new CreatedResult(string.Empty, operation);
        }
    }
}
