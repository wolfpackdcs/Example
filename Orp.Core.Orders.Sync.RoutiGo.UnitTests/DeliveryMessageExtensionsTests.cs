using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Messages;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Orp.Core.Orders.Sync.RoutiGo.UnitTests
{
    public class DeliveryMessageExtensionsTests
    {
        [Fact]
        public void DeliveredItems_UniqueDeliveryIds_Validate()
        {
            //Arrange.
            List<DeliveredItem> deliveredItems = new List<DeliveredItem>()
            {
                new DeliveredItem( new string[] {"1", "SKU1", "2", "3"}),
                new DeliveredItem( new string[] {"4", "SKU2", "5", "6" })
            };

            //Act.
            IEnumerable<int> actual = deliveredItems.UniqueDeliveryIds();

            //Asert.
            actual.ShouldNotBeNull();
            actual.Count().ShouldBe(2);
            actual.ShouldContain(1);
            actual.ShouldContain(4);
        }
    }
}
