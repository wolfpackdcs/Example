using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orp.Core.Orders.Sync.RoutiGo.Function.Configuration;
using Orp.Core.Orders.Sync.RoutiGo.Function.Extensions;

[assembly: FunctionsStartup(typeof(Orp.Core.Orders.Sync.RoutiGo.Function.Startup))]
namespace Orp.Core.Orders.Sync.RoutiGo.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Initialize function configuration: 
            var configuration = new ConfigurationHelper(builder
                .Services
                .BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>()
                .Value
                .AppDirectory);

            // Configure logging inside the .NET Core DI container: 
            builder.Services.AddLog4NetLogging(configuration);

            // Configure services inside the .NET Core DI container: 
            builder.Services.ConfigureNinjectKernel(configuration);
        }
    }
}
