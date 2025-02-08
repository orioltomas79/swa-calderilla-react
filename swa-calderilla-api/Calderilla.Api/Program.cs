using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Calderilla.DataAccess;
using Calderilla.Services;

namespace Calderilla.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            // ASP.NET Core integration
            builder.ConfigureFunctionsWebApplication();

            // Add middleware
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#start-up-and-configuration
            builder.UseMiddleware<ValidateUserMiddleware>();
            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            // Dependecy injection
            // https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            // Services
            builder.Services.AddSingleton<IOperationsService, OperationsService>();
            // DataAccess
            builder.Services.AddSingleton<IBlobRepository, BlobRepository>();
            builder.Services.AddSingleton<IOperationsRepository, OperationsRepository>();

            // Build and Run the host
            builder.Build().Run();
        }
    }
}
