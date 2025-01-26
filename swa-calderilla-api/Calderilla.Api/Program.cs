using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Calderilla.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.ConfigureFunctionsWebApplication();

            builder.UseMiddleware<ExceptionHandlingMiddleware>();

            builder.Build().Run();
        }
    }
}
