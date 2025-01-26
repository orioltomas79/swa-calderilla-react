using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Functions.Dev
{
    public class Get500ErrorFunction
    {
        private readonly ILogger<Get500ErrorFunction> _logger;

        public Get500ErrorFunction(ILogger<Get500ErrorFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetThrowException")]

        [OpenApiOperation(operationId: nameof(GetThrowException), tags: [Constants.DevEndpointsTag], Summary = "Returns an exception")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns an exception")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 500 error message")]
        public IActionResult GetThrowException([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            throw new Exception("This is an exception");
        }

        [Function("Get404Error")]
        [OpenApiOperation(operationId: nameof(Get404Error), tags: [Constants.DevEndpointsTag], Summary = "Returns a 404 error")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 404 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 500 error message")]

        public IActionResult Get404Error([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            return new NotFoundObjectResult($"Item was not found.");
        }
    }
}
