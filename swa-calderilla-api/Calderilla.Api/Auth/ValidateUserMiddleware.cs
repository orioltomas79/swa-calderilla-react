using System.Net;
using Calderilla.Api.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Calderilla.Api.Auth
{
    /// <summary>
    /// ValidateUserMiddleware.cs is a custom middleware that validates user authentication and authorization.
    /// It uses StaticWebAppsAuth to extract a ClaimsPrincipal from the HTTP request. 
    /// If the user claims are missing or the user lacks the required role, it returns an unauthorized response with appropriate problem details. 
    /// The middleware logs unauthorized access attempts and ensures only authorized users can proceed to the next middleware or function.
    /// </summary>
    public class ValidateUserMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger _logger;

        public ValidateUserMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ValidateUserMiddleware>();
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            HttpContext? httpContext = context.GetHttpContext();

            if (httpContext == null)
            {
                _logger.LogError("HttpContext is null. Unable to process the request.");
                return;
            }

            var req = httpContext.Request;
            var claimsPrincipal = StaticWebAppsAuth.GetClaimsPrincipal(req);

            if (claimsPrincipal.Identity == null)
            {
                var problemDetails = ProblemDetailsHelper.UnauthorizedProblemDetails(req, "Unable to get the user claims", string.Empty);

                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(problemDetails.Value);
                return;
            }

            var userRoles = claimsPrincipal.GetRoles();

            if (!userRoles.Contains("user"))
            {
                _logger.LogInformation("User is not authorized");

                var problemDetails = ProblemDetailsHelper.UnauthorizedProblemDetails(req, "User is not authorized", string.Empty);

                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(problemDetails.Value);
                return;
            }

            await next(context);
        }
    }
}
