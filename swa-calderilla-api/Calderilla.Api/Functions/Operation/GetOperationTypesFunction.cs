using Calderilla.Api.Functions.Dev;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Calderilla.Api.ErrorHandling;
using Calderilla.Api.Auth;
using Calderilla.Services.Operations;

namespace Calderilla.Api.Functions.Operation
{
    public class GetOperationTypesFunction
    {
        private readonly ILogger<GetOperationTypesFunction> _logger;

        public GetOperationTypesFunction(ILogger<GetOperationTypesFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(GetOperationTypes))]
        [OpenApiOperation(operationId: nameof(GetOperationTypes), tags: [ApiEndpoints.OperationsEndpointsTag], Summary = "Returns a list of operation types")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Domain.OperationType>), Description = "Returns a list of operations")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.Unauthorized, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 401 error message")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.InternalServerError, contentType: "application/json", bodyType: typeof(ProblemDetails), Description = "Returns a 500 error message")]
        public IActionResult GetOperationTypes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetOperationTypes)] HttpRequest req)
        {
            var listOperationTypes = new List<Domain.OperationType>
            {
                new() { Id = 0, Name = "Inversió" },
                new() { Id = 1, Name = "Amazon" },
                new() { Id = 2, Name = "Cardedeu - Aigua" },
                new() { Id = 3, Name = "Cardedeu - Comunitat" },
                new() { Id = 4, Name = "Cardedeu - Coses casa" },
                new() { Id = 5, Name = "Cardedeu - Gas" },
                new() { Id = 6, Name = "Cardedeu - Hipoteca" },
                new() { Id = 7, Name = "Cardedeu - Impostos" },
                new() { Id = 8, Name = "Cardedeu - Llum" },
                new() { Id = 9, Name = "Cardedeu - Seguro" },
                new() { Id = 10, Name = "Carn/Peix" },
                new() { Id = 11, Name = "Carnets Barça" },
                new() { Id = 12, Name = "Casal" },
                new() { Id = 13, Name = "Compres Nenes" },
                new() { Id = 14, Name = "Compres Ori" },
                new() { Id = 15, Name = "Cotxe" },
                new() { Id = 16, Name = "Estudis Nenes" },
                new() { Id = 17, Name = "Farmacia" },
                new() { Id = 18, Name = "Ferreteria" },
                new() { Id = 19, Name = "Fisio" },
                new() { Id = 20, Name = "Formació" },
                new() { Id = 21, Name = "Gasolina" },
                new() { Id = 22, Name = "Gym" },
                new() { Id = 23, Name = "Ingrés Núria" },
                new() { Id = 24, Name = "Ingrés Oriol" },
                new() { Id = 25, Name = "Metge" },
                new() { Id = 26, Name = "Metge Nenes" },
                new() { Id = 27, Name = "Multa" },
                new() { Id = 28, Name = "Netflix" },
                new() { Id = 29, Name = "Neteja/Cangur" },
                new() { Id = 30, Name = "Nomina GFT" },
                new() { Id = 31, Name = "Nutricionista" },
                new() { Id = 32, Name = "Oci" },
                new() { Id = 33, Name = "Padel" },
                new() { Id = 34, Name = "Parking" },
                new() { Id = 35, Name = "Pelu" },
                new() { Id = 36, Name = "Peatges" },
                new() { Id = 37, Name = "Regals" },
                new() { Id = 38, Name = "Renta" },
                new() { Id = 39, Name = "Restaurant" },
                new() { Id = 40, Name = "Roba" },
                new() { Id = 41, Name = "Rocky" },
                new() { Id = 42, Name = "Spotify" },
                new() { Id = 43, Name = "St Leo - Aigua" },
                new() { Id = 44, Name = "St Leo - Comunitat" },
                new() { Id = 45, Name = "St Leo - Coses casa" },
                new() { Id = 46, Name = "St Leo - Hipoteca" },
                new() { Id = 47, Name = "St Leo - Impostos" },
                new() { Id = 48, Name = "St Leo - Llum" },
                new() { Id = 49, Name = "St Leo - Seguro" },
                new() { Id = 50, Name = "Super" }
            };

            return new OkObjectResult(listOperationTypes);
        }
    }
}
