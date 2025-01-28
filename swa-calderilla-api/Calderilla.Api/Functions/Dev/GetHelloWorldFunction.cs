using System.Net;
using Calderilla.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Calderilla.Api.Functions.Dev
{
    public class GetHelloWorldFunction
    {
        private readonly ILogger<GetHelloWorldFunction> _logger;
        private readonly Service1 _service1;

        public GetHelloWorldFunction(ILogger<GetHelloWorldFunction> logger, Service1 service1)
        {
            _logger = logger;
            _service1 = service1;
        }

        [Function(nameof(GetHelloWorldMessage))]
        [OpenApiOperation(operationId: nameof(GetHelloWorldMessage), tags: [ApiEndpoints.DevEndpointsTag], Summary = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 500 error message")]
        public IActionResult GetHelloWorldMessage([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetHelloWorldMessage)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            var userName = claimsPrincipal.GetUserId();

            var message = _service1.GetMessage(userName);

            return new JsonResult(message);
        }
    }
}
