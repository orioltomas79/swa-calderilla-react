using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calderilla.Api.ErrorHandling
{
    public static class ProblemDetailsHelper
    {
        public static ObjectResult NotFoundProblemDetails(
            HttpRequest httpRequest,
            string title,
            string detail)
        {
            return GetProblemDetailsResponse(
                httpRequest, 
                HttpStatusCode.NotFound, 
                "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4", 
                title, 
                detail);
        }

        private static ObjectResult GetProblemDetailsResponse(
            HttpRequest httpRequest,
            HttpStatusCode statusCode,
            string type,
            string title,
            string detail)
        {
            var problemDetails = new ProblemDetails
            {
                Type = type,
                Title = title,
                Detail = detail,
                Status = (int)statusCode,
                Instance = httpRequest.Path
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)statusCode,
                ContentTypes = { "application/problem+json" }
            };

        }
    }
}