using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions
{
    public static class DeliveryMessageExtensions
    {
        internal static bool HasDeliveredItems(this DeliveryMessage message)
             => message.DeliveredItems.Any();

        internal static IEnumerable<int> UniqueDeliveryIds(this List<DeliveredItem> deliveredItems)
            => deliveredItems?.GroupBy(x => x.DeliveryId).Select(x => x.Key).Distinct() ?? Enumerable.Empty<int>();
    }
}
