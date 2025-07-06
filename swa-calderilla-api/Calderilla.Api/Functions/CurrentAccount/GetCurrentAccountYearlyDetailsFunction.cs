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

namespace Calderilla.Api.Functions.CurrentAccount;

public class GetCurrentAccountYearlyDetailsFunction(ILogger<GetCurrentAccountYearlyDetailsFunction> logger, IAccountsService accountsService)
{
    private readonly ILogger<GetCurrentAccountYearlyDetailsFunction> _logger = logger;

    private readonly IAccountsService _accountsService = accountsService;

    [Function(nameof(GetCurrentAccountYearlyDetails))]
    [OpenApiOperation(operationId: nameof(GetCurrentAccountYearlyDetails), tags: [ApiEndpoints.CurrentAccountsEndpointsTag], Summary = "Returns the yearly details for the current account.")]
    [OpenApiParameter(name: "accountId", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Description = "The current account")]
    [OpenApiParameter(name: "year", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The year of the operations")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(GetCurrentAccountYearlyDetailsResponse), Description = "Returns the yearly details for the current account.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
    public async Task<IActionResult> GetCurrentAccountYearlyDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetCurrentAccountYearlyDetails)] HttpRequest req, Guid accountId, int year)
    {
        var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

        var types = await _accountsService.GetYearlyTypeSummaryAsync(claimsPrincipal.GetName(), accountId, year);

        var result = new GetCurrentAccountYearlyDetailsResponse
        {
            Types = types
        };

        return new OkObjectResult(result);
    }
}
