using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Functions.Dev
{
    public class GetInternalServerErrorFunction
    {
        private readonly ILogger<GetInternalServerErrorFunction> _logger;

        public GetInternalServerErrorFunction(ILogger<GetInternalServerErrorFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetInternalServerError")]

        [OpenApiOperation(operationId: nameof(GetInternalServerError), tags: [ApiEndpoints.DevEndpointsTag], Summary = "Returns an exception")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns an exception")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public IActionResult GetInternalServerError([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetInternalServerError)] HttpRequest req)
        {
            throw new Exception("This is an unhandled exception");
        }
    }
}
