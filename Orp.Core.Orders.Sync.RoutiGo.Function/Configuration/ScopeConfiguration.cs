using Encrypted.Configuration.Util.Attributes;
using Wolfpack.MultiTenant.TokenProvider.Models;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Configuration
{
    public class ScopeConfiguration
    {
        public string ScopeId { get; set; }

        [EncryptedObject]
        public TenantOAuth2Configuration TenantOAuth2Configuration { get; set; }

        public string ExportDeliveryUploadDirectory { get; set; }
    }
}
