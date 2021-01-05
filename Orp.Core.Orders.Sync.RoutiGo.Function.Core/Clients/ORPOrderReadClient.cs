using Orp.Core.Clients.Base.Security;
using Orp.Core.Orders.DTO.Orders.V2;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Clients
{
    public class ORPOrderReadClient : BaseSecurityClient, IOrderReadClient
    {
        private readonly string _scopeId;

        public ORPOrderReadClient(string baseAddress, OAuth2Configuration configuration, string scopeId) : base(baseAddress, configuration)
        {
            if (string.IsNullOrWhiteSpace(scopeId))
                throw new ArgumentNullException(nameof(scopeId));

            _scopeId = scopeId;
        }

        public Task<OrderDTO> GetOrderAsync(string orderNumber)
            => GetAsync<OrderDTO>($"v2/orders/{orderNumber}?scopeId={_scopeId}");
    }
}
