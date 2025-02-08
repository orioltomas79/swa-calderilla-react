using Calderilla.Api.Functions.Dev;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Calderilla.Api.Functions.CurrentAccount
{
    public class GetCurrentAccountFunction
    {
        private readonly ILogger<GetCurrentAccountFunction> _logger;

        public GetCurrentAccountFunction(ILogger<GetCurrentAccountFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(GetCurrentAccounts))]
        [OpenApiOperation(operationId: nameof(GetCurrentAccounts), tags: [ApiEndpoints.CurrentAccountsEndpointsTag], Summary = "Returns a list of current accounts")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Domain.CurrentAccount>), Description = "Returns a list of current accounts")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public IActionResult GetCurrentAccounts([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req)
        {
            var result = new List<Domain.CurrentAccount>()
            {
                new ()
                {
                    Id = 1,
                    Name = "Oriol",
                    Type = "Ing"
                },
                new ()
                {
                    Id = 2,
                    Name = "Comuna",
                    Type = "Sabadell"
                }
            };
            return new OkObjectResult(result);
        }
    }
}
