using Orp.Core.Orders.DTO.Orders.V2;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Models;
using System.Linq;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Mapping
{
    public class ToDeliveryCSVMapper
    {
        internal DeliveryCSV Map(string orderNumber, AddressDTO address, string trackAndTrace)
        {
            return new DeliveryCSV()
            {
                NrOfColli = "1",
                OrderNumber = orderNumber,
                CustomerName = address?.LastName,
                Street = address?.Address1,
                HouseNumber = MapToHouseNumber(address?.Address2, address?.Address3),
                PostalCode = address?.PostalCode,
                City = address?.City,
                Country = "NL",
                PhoneNumber = address?.Phone,
                Email = address?.Email,
                Timeframe = string.Empty,
                Instructions = "geen bijzonderheden",
                DeliveryDate = string.Empty,
                Customer = "Xenos",
                Barcode = trackAndTrace,
                ColliType = "Colli"
            };
        }

        internal string MapToHouseNumber(params string[] addressParts)
          => string.Join(" ", addressParts.Where(x => !string.IsNullOrWhiteSpace(x)));
    }
}
