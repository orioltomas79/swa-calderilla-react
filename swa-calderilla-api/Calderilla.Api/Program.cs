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

            // Integrate ASP.NET Core features into Azure Function apps.
            // This method sets up the function host to leverage ASP.NET Core's middleware pipeline, routing, and dependency injection capabilities,
            // enabling a more unified development experience when building HTTP-triggered functions
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#aspnet-core-integration
            builder.ConfigureFunctionsWebApplication();

            // Add middleware
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#start-up-and-configuration
            AddMiddleware(builder);

            // Dependecy injection
            // https://learn.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            AddMyServices(builder);

            // Build and Run the host
            builder.Build().Run();
        }

        private static void AddMiddleware(FunctionsApplicationBuilder builder)
        {
            builder.UseMiddleware<ValidateUserMiddleware>();
            builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        private static void AddMyServices(FunctionsApplicationBuilder builder)
        {
            // Services
            builder.Services.AddSingleton<IOperationsService, OperationsService>();
         
            // DataAccess
            builder.Services.AddSingleton<IBlobRepository, BlobRepository>();
            builder.Services.AddSingleton<IOperationsRepository, OperationsRepository>();
        }
    }
}
