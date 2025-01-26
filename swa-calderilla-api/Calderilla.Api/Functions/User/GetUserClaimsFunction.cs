using System.Net;
using Calderilla.Api.Functions.Dev;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Functions.User
{
    public class GetUserClaimsFunction
    {
        private readonly ILogger<GetUserClaimsFunction> _logger;

        public GetUserClaimsFunction(ILogger<GetUserClaimsFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(GetUserClaims))]
        [OpenApiOperation(operationId: nameof(GetUserClaims), tags: [ApiEndpoints.UsersEndpointsTag], Summary = "Gets current user claims")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserClaims), Description = "The OK response")]
        public IActionResult GetUserClaims([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetUserClaims)] HttpRequest req)
        {
            _logger.LogInformation("GetUserClaims function processed a request.");

            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            if (claimsPrincipal.Identity == null)
            {
                return new UnauthorizedResult();
            }

            var userClaims = new UserClaims()
            {
                Name = claimsPrincipal.Identity.Name!,
                AuthType = claimsPrincipal.Identity.AuthenticationType!
            };

            return new OkObjectResult(userClaims);
        }

        public class UserClaims
        {
            public required string Name { get; set; }
            public required string AuthType { get; set; }
        }
    }
}
