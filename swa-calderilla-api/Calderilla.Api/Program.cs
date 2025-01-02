using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

namespace Calderilla.Api
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.ConfigureFunctionsWebApplication();

            builder.Build().Run();
        }
    }
}
