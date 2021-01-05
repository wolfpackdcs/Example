using Orp.Core.FTP.Interfaces;
using Orp.Core.Logging.Interfaces;
using Orp.Core.Orders.DTO.Orders.V2;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Interfaces;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Mapping;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Messages;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Orp.Core.Orders.Sync.RoutiGo.UnitTests")]

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core
{
    public class DeliverySyncHandler : IHandleMessage<DeliveryMessage>
    {
        private readonly ILog _logger;
        private readonly IOrderReadClient _orderReadClient;
        private readonly ToDeliveryCSVMapper _mapper = new ToDeliveryCSVMapper();
        private readonly IFTPClient _ftpClient;
        private readonly string _exportDeliveryUploadDirectory;

        public DeliverySyncHandler(ILog logger,
                                   IOrderReadClient orderReadClient,
                                   IFTPClient ftpClient,
                                   string exportDeliveryUploadDirectory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderReadClient = orderReadClient ?? throw new ArgumentNullException(nameof(orderReadClient));
            _ftpClient = ftpClient ?? throw new ArgumentNullException(nameof(ftpClient));

            if (string.IsNullOrWhiteSpace(nameof(exportDeliveryUploadDirectory)))
                throw new ArgumentNullException(nameof(exportDeliveryUploadDirectory));

            _exportDeliveryUploadDirectory = exportDeliveryUploadDirectory;
        }

        public async Task HandleAsync(DeliveryMessage message)
        {
            _logger.Information($"Started exporting order {message.OrderNumber} to RoutiGo.");

            //Only act on actual Delivered items. Mispicks and Returns are not relevant. 
            //The reason for this check, and not sending the correct event,  is that it is/was a choice between extra IO or inconsistent behavior.
            if (message.HasDeliveredItems())
            {
                OrderDTO order = await _orderReadClient.GetOrderAsync(message.OrderNumber);

                foreach (int deliveryId in message.DeliveredItems.UniqueDeliveryIds())
                {
                    if (order.Shipments?.FirstOrDefault(x => x.Deliveries?.Any(y => y.Id == deliveryId) is true) is ShipmentDTO shipment
                      && shipment.CanBeExported())
                    {
                        DeliveryDTO delivery = shipment.Deliveries.First(x => x.Id == deliveryId);

                        _logger.Information($"Mapping order {order.OrderNumber} data to Delivery CSV data.");

                        DeliveryCSV csv = _mapper.Map(order.OrderNumber, shipment.ShippingAddress, delivery.TrackNTrace);

                        Stream stream = csv.WriteToCSV<DeliveryCSV, DeliveryCSVClassMap>();

                        await _ftpClient.UploadFileAsync(stream, GetPath(GetFileName(order.OrderNumber)));
                    }
                    else
                    {
                        _logger.Information($"Order {message.OrderNumber} is not exported to RoutiGo. The postalcode prefix check for NL orders does not match.");
                    }
                }
            }
            else
            {
                _logger.Information($"No DeliveredItems available for order {message.OrderNumber}. Export to RoutiGo will not be done.");
            }

            _logger.Information($"Completed exporting order {message.OrderNumber} to RoutiGo.");
        }

        internal string GetFileName(string orderNumber)
            => $"Xenos_{orderNumber}.{DateTime.UtcNow:yyyyMMddHHmmssffffff}.csv";

        internal string GetPath(string filename)
         => $"{_exportDeliveryUploadDirectory}/{filename}";
    }
}
