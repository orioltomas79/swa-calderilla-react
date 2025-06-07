using Calderilla.Api.Functions.Dev;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Services.Accounts;
using Calderilla.Api.Auth;

namespace Calderilla.Api.Functions.CurrentAccount
{
    public class GetCurrentAccountYearlySummaryFunction(ILogger<GetCurrentAccountYearlySummaryFunction> logger, IAccountsService accountsService)
    {
        private readonly ILogger<GetCurrentAccountYearlySummaryFunction> _logger = logger;

        private readonly IAccountsService _accountsService = accountsService;

        [Function(nameof(GetCurrentAccountYearlySummary))]
        [OpenApiOperation(operationId: nameof(GetCurrentAccountYearlySummary), tags: [ApiEndpoints.CurrentAccountsEndpointsTag], Summary = "Returns a yearly summary for the current account")]
        [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
        [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetCurrentAccountYearlySummaryResponse), Description = "Returns a yearly summary for the current account")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public async Task<IActionResult> GetCurrentAccountYearlySummary([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetCurrentAccountYearlySummary)] HttpRequest req, Guid accountId, int year)
        {
            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            var months = await _accountsService.GetYearlySummaryAsync(claimsPrincipal.GetName(), accountId, year);

            var result = new GetCurrentAccountYearlySummaryResponse
            {
                Months = months
            };

            return new OkObjectResult(result);
        }
    }
}
