using Orp.Core.Orders.DTO.Orders.V2;
using System;
using System.Linq;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions
{
    internal static class ShipmentDTOExtensionMethods
    {
        private static string[] _postalCodesPrefixes = Environment.GetEnvironmentVariable("XENOS_POSTALCODE_PREFIXES")?.Split(";");

        //This is custom logic for Xenos.
        internal static bool CanBeExported(this ShipmentDTO shipment)
        => shipment.ShippingAddress?.CountryCode == "NL" && _postalCodesPrefixes.Any(x => shipment.ShippingAddress?.PostalCode?.StartsWith(x) ?? false);
    }
}
