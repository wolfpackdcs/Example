using Encrypted.Configuration.Util.Attributes;
using System.Collections.Generic;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Configuration
{
    public class TenantConfigurationHolder
    {
        [EncryptedProperty]
        public List<TenantConfiguration> TenantConfigurations { get; set; }
    }

    public class TenantConfiguration
    {
        public string TenantId { get; set; }

        [EncryptedProperty]
        public List<ScopeConfiguration> ScopeConfigurations { get; set; }
    }
}
