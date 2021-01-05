using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Ninject;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Messages;
using System.Threading.Tasks;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Functions
{
    public class DeliverySyncFunction : FunctionBase
    {
        public DeliverySyncFunction(IKernel kernel) : base(kernel)
        {
        }

        [FunctionName("Delivery_Sync")]
        public async Task Run([ServiceBusTrigger("opm_task_order_delivery_sync_routigo", "Orp.Order.Sync.RoutiGo", Connection = "ServiceBusConnection")] Message msg,
                              Binder binder,
                              [ServiceBus(queueOrTopicName: "opm_taskfinished", EntityType = EntityType.Topic, Connection = "ServiceBusConnection")] IAsyncCollector<Message> resultMessages)
        {
            await resultMessages.AddAsync(await RunAsMessageHandlerProcessManagerTaskAsync<DeliveryMessage, DeliverySyncHandler>(msg, binder));
        }
    }
}
