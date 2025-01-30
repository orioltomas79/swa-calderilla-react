using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calderilla.Api.ErrorHandling
{
    public static class ValidationProblemDetailsHelper
    {
        public static ObjectResult ValidationProblemDetails(
            HttpRequest httpRequest,
            string title,
            string detail,
            IDictionary<string, string[]> errors)
        {
            return GetValidationProblemDetailsResponse(
                httpRequest,
                HttpStatusCode.BadRequest,
                "https://datatracker.ietf.org/doc/html/rfc9110#name-400-bad-request",
                title,
                detail,
                errors);
        }

        private static ObjectResult GetValidationProblemDetailsResponse(
            HttpRequest httpRequest,
            HttpStatusCode statusCode,
            string type,
            string title,
            string detail,
            IDictionary<string, string[]> errors)
        {
            var validationProblemDetails = new ValidationProblemDetails
            {
                Type = type,
                Title = title,
                Detail = detail,
                Status = (int)statusCode,
                Instance = httpRequest.Path,
                Errors = errors
            };

            return new ObjectResult(validationProblemDetails)
            {
                StatusCode = (int)statusCode,
                ContentTypes = { "application/json" }
            };

        }
    }
}