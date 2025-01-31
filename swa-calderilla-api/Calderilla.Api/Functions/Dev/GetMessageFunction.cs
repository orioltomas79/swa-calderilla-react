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
        private readonly IGetMessageService _getMessageService;

        public GetMessageFunction(ILogger<GetMessageFunction> logger, IGetMessageService getMessageService)
        {
            _logger = logger;
            _getMessageService = getMessageService;
        }

        [Function(nameof(GetMessage))]
        [OpenApiOperation(operationId: nameof(GetMessage), tags: [ApiEndpoints.DevEndpointsTag], Summary = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public IActionResult GetMessage([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetMessage)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            var userName = claimsPrincipal.GetUserId();

            var message = _getMessageService.GetMessage(userName);

            return new OkObjectResult(message);
        }
    }
}
