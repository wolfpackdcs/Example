using ProcessManager.Messages.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Messages
{
    public class DeliveryMessage : ASB_TaskMessage
    {
        public string OrderNumber => (ProcessData?.ContainsKey("orderNumber") ?? false) ? ProcessData["orderNumber"] : throw new NotSupportedException("OrderNumber has not been provided. Message cannot be handled.");
        public List<DeliveredItem> DeliveredItems => ProcessData.ContainsKey("updatedDeliveries") ? CustomCSVParser.ReadUpdatedDeliveries(ProcessData["updatedDeliveries"]) : new List<DeliveredItem>();
        public List<MispickedItem> MispickedItems => ProcessData.ContainsKey("mispickedItems") ? CustomCSVParser.ReadMispickedItems(ProcessData["mispickedItems"]) : new List<MispickedItem>();
        public List<ReturnedItem> ReturnedItems => ProcessData.ContainsKey("returnedItems") ? CustomCSVParser.ReadReturnedItems(ProcessData["returnedItems"]) : new List<ReturnedItem>();
    }

    public static class CustomCSVParser
    {
        public static List<DeliveredItem> ReadUpdatedDeliveries(string lines)
         => string.IsNullOrWhiteSpace(lines) ? new List<DeliveredItem>() : GetLines(lines).Select(x => new DeliveredItem(GetFields(x))).ToList();

        public static List<MispickedItem> ReadMispickedItems(string lines)
          => string.IsNullOrWhiteSpace(lines) ? new List<MispickedItem>() : GetLines(lines).Select(x => new MispickedItem(GetFields(x))).ToList();

        public static List<ReturnedItem> ReadReturnedItems(string lines)
          => string.IsNullOrWhiteSpace(lines) ? new List<ReturnedItem>() : GetLines(lines).Select(x => new ReturnedItem(GetFields(x))).ToList();

        internal static string[] GetLines(string lines)
          => lines.Split(';', StringSplitOptions.RemoveEmptyEntries);

        internal static string[] GetFields(string line)
            => line.Split(':', StringSplitOptions.RemoveEmptyEntries);
    }

    public class DeliveredItem
    {
        public int DeliveryId { get; set; }
        public string SKU { get; set; }
        public string OrderLineExternalId { get; set; }
        public int Quantity { get; set; }

        public DeliveredItem(string[] line)
        {
            DeliveryId = int.Parse(line[0]);
            SKU = line[1];
            OrderLineExternalId = line[2];
            Quantity = int.Parse(line[3]);
        }
    }

    public class ReturnedItem
    {
        public string SKU { get; set; }
        public string OrderLineExternalId { get; set; }
        public int Quantity { get; set; }

        public ReturnedItem(string[] line)
        {
            SKU = line[0];
            OrderLineExternalId = line[1];
            Quantity = int.Parse(line[2]);
        }
    }

    public class MispickedItem
    {
        public string SKU { get; set; }
        public string OrderLineExternalId { get; set; }
        public int Quantity { get; set; }

        public MispickedItem(string[] line)
        {
            SKU = line[0];
            OrderLineExternalId = line[1];
            Quantity = int.Parse(line[2]);
        }
    }
}
