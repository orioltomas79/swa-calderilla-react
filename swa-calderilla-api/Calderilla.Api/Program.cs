using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Calderilla.Api.Repo;

namespace Calderilla.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.ConfigureFunctionsWebApplication();

            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            builder.Services.AddSingleton<IBlobRepo, BlobRepo>();

            builder.Build().Run();
        }
    }
}
