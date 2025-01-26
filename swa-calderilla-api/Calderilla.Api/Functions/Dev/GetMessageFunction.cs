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
    public class GetMessageFunction
    {
        private readonly ILogger<GetMessageFunction> _logger;
        private readonly Service1 _service1;

        public GetMessageFunction(ILogger<GetMessageFunction> logger, Service1 service1)
        {
            _logger = logger;
            _service1 = service1;
        }

        [Function(nameof(GetMessage))]
        [OpenApiOperation(operationId: nameof(GetMessage), tags: [Constants.DevEndpointsTag], Summary = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 500 error message")]
        public IActionResult GetMessage([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            var userName = claimsPrincipal.GetUserId();

            var message = _service1.GetMessage(userName);

            return new JsonResult(message);
        }
    }
}
