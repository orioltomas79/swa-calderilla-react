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
                "https://datatracker.ietf.org/doc/html/rfc9110#name-404-not-found",
                title,
                detail);
        }

        public static ObjectResult UnauthorizedProblemDetails(
            HttpRequest httpRequest,
            string title,
            string detail)
        {
            return GetProblemDetailsResponse(
                httpRequest,
                HttpStatusCode.Unauthorized,
                "https://datatracker.ietf.org/doc/html/rfc9110#name-401-unauthorized",
                title,
                detail);
        }

        public static ObjectResult InternalServerError(
            HttpRequest httpRequest,
            string detail)
        {
            return GetProblemDetailsResponse(
                httpRequest,
                HttpStatusCode.InternalServerError,
                "https://datatracker.ietf.org/doc/html/rfc9110#name-500-internal-server-error",
                "An internal server error occurred.",
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
                ContentTypes = { "application/json" }
            };

        }
    }
}