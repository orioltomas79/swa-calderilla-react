using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

/// <summary>
///  Behavior of the Middleware:
///  Error Logging: Any unhandled exception will be caught by the middleware, logged, and returned as a structured response.
///  HTTP Response: The middleware ensures that the client receives a meaningful error response (e.g., status code 500 with a JSON body).
/// </summary>
public class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            var httpResponse = context.GetHttpResponseData();
            if (httpResponse == null)
            {
                var request = await context.GetHttpRequestDataAsync();
                if (request != null)
                {
                    httpResponse = request.CreateResponse();
                }
            }

            if (httpResponse != null)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = (int?)HttpStatusCode.InternalServerError,
                    Title = "An internal server error occurred.",
                    Detail = ex.Message,
                    Instance = context.InvocationId.ToString()
                };

                httpResponse.StatusCode = HttpStatusCode.InternalServerError;
                await httpResponse.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
