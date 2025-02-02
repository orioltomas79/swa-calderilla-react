using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calderilla.Api.ErrorHandling
{
    public static class ValidationProblemDetailsHelper
    {
        public static ObjectResult ValidationProblemDetails(
            HttpRequest httpRequest,
            IDictionary<string, string[]> errors)
        {
            var validationProblemDetails = new ValidationProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request",
                Title = "Validation errors occurred.",
                Detail = "Please refer to the errors property for additional details.",
                Status = (int?)HttpStatusCode.BadRequest,
                Instance = httpRequest.Path,
                Errors = errors
            };

            return new ObjectResult(validationProblemDetails)
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ContentTypes = { "application/json" }
            };
        }
    }
}