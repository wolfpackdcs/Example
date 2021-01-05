using System;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Configuration
{
    internal class ConfigurationHelper
    {
        internal string AppBaseDirectory { get; set; }
        internal string ConfigCertThumbprint => Environment.GetEnvironmentVariable("CONFIG_CERT_THUMBPRINT");
        internal string CurrentEnvironment => Environment.GetEnvironmentVariable("ENVIRONMENT");
        internal string OrderReadAPIUri => Environment.GetEnvironmentVariable("ORDER_READ_API_URI");
        internal string IdentityServerUri => Environment.GetEnvironmentVariable("IDENTITY_SERVER_URI");

        public ConfigurationHelper(string appBaseDirectory)
        {
            AppBaseDirectory = string.IsNullOrWhiteSpace(appBaseDirectory)
                ? throw new ArgumentNullException(nameof(appBaseDirectory))
                : appBaseDirectory;
        }
    }
}
