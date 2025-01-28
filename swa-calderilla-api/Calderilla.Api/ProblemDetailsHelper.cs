using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Calderilla.Api
{
    public static class ProblemDetailsHelper
    {
        public static async Task WriteProblemDetailsResponseAsync(
            HttpRequest httpRequest,
            HttpStatusCode statusCode,
            string title,
            string detail = null,
            string type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            string instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Type = type,
                Title = title,
                Detail = detail,
                Status = (int)statusCode,
                Instance = instance
            };

            var httpResponse = httpRequest.HttpContext.Response;
            httpResponse.StatusCode = (int)statusCode;
            httpResponse.ContentType = "application/problem+json";

            await httpResponse.WriteAsJsonAsync(problemDetails);
        }
    }
}