using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Orp.Core.Orders.Sync.RoutiGo.Function.Configuration;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Extensions
{
    /// <summary>
    /// Instantiates and injects the Ninject kernel into the .NET Core DI container. 
    /// This allows for multi-tenant configuration. 
    /// </summary>
    internal static class NinjectExtensions
    {
        internal static IServiceCollection ConfigureNinjectKernel(this IServiceCollection services,
            ConfigurationHelper configuration)
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(new NinjectBindings(configuration));

            return services.AddSingleton(kernel);
        }
    }
}
