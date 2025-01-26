using System.Net;
using Calderilla.Api.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Calderilla.Api.Functions
{
    public class Function1
    {
        private const string OtherEndpointsTag = "OtherEndpoints";

        private readonly ILogger<Function1> _logger;
        private readonly IBlobRepo _blobRepo;

        public Function1(ILogger<Function1> logger, IBlobRepo blobRepo)
        {
            _logger = logger;
            _blobRepo = blobRepo;
        }

        [Function(nameof(GetMessage))]
        [OpenApiOperation(operationId: nameof(GetMessage), tags: [OtherEndpointsTag], Summary = "Returns a message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a message")]
        public IActionResult GetMessage([Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            var userName = StaticWebAppsAuth.GetUserId(claimsPrincipal);

            var message = Services.Service1.GetMessage(userName);

            return new JsonResult(message);
        }

        [Function(nameof(GetMessageBlobRepo))]
        [OpenApiOperation(operationId: nameof(GetMessageBlobRepo), tags: [OtherEndpointsTag], Summary = "Returns a message from the blob repo")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a message from the blob repo")]
        public IActionResult GetMessageBlobRepo([Microsoft.Azure.Functions.Worker.HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            
            var message = _blobRepo.GetMessage();

            return new JsonResult(message);
        }
    }
}
