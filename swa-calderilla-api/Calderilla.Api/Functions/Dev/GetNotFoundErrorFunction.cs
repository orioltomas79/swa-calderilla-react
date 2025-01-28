using System.Net;
using Calderilla.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Functions.Dev
{
    public class GetNotFoundErrorFunction
    {
        private readonly ILogger<GetNotFoundErrorFunction> _logger;

        public GetNotFoundErrorFunction(ILogger<GetNotFoundErrorFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetNotFoundError")]
        [OpenApiOperation(operationId: nameof(GetNotFoundError), tags: [ApiEndpoints.DevEndpointsTag], Summary = "Returns a 404 error")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 404 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 500 error message")]

        public IActionResult GetNotFoundError([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetNotFoundError)] HttpRequest req)
        {
            return ProblemDetailsHelper.NotFoundProblemDetails(req, "The item was not found", string.Empty);
        }
    }
}
