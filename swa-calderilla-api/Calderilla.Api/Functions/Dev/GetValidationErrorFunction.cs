using System.Net;
using Calderilla.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Functions.Dev
{
    public class GetValidationErrorFunction
    {
        private readonly ILogger<GetNotFoundErrorFunction> _logger;

        public GetValidationErrorFunction(ILogger<GetNotFoundErrorFunction> logger)
        {
            _logger = logger;
        }

        [Function("GetValidationError")]
        [OpenApiOperation(operationId: nameof(GetValidationError), tags: [ApiEndpoints.DevEndpointsTag], Summary = "Returns a 400 error")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ValidationProblemDetails), Description = "Returns a 400 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]

        public IActionResult GetValidationError([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetValidationError)] HttpRequest req)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "Email", new[] { "The Email field is required.", "The Email field must be a valid email address." } },
                { "Password", new[] { "The Password field must be at least 8 characters long.", "The Password field must contain at least one uppercase letter, one lowercase letter, and one number." } }
            };
            return ValidationProblemDetailsHelper.ValidationProblemDetails(req, "The item was not found", string.Empty, errors);
        }
    }
}
