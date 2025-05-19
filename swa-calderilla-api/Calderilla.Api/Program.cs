using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Calderilla.DataAccess;
using Calderilla.Api.Auth;
using Calderilla.Services.Operations;
using Calderilla.Services.Banks.Ing;
using Calderilla.Services.Banks.Sabadell;

namespace Calderilla.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            // Integrate ASP.NET Core features into Azure Function apps.
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#aspnet-core-integration
            builder.ConfigureFunctionsWebApplication();

            // Add middlewares
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#middleware
            AddMiddleware(builder);

            // Dependecy injection configuration
            // https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#dependency-injection
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
            builder.Services.AddSingleton<IIngService, IngService>();
            builder.Services.AddSingleton<ISabadellService, SabadellService>();
        }
    }
}
