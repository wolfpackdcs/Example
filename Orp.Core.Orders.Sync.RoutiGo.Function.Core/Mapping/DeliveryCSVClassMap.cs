using CsvHelper.Configuration;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Models;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Core.Mapping
{
    public class DeliveryCSVClassMap : ClassMap<DeliveryCSV>
    {
        public DeliveryCSVClassMap()
        {
            Map(x => x.NrOfColli).Index(0).Name("Colli aantal");
            Map(x => x.OrderNumber).Index(1).Name("Order nummer");
            Map(x => x.CustomerName).Index(2).Name("Naam klant");
            Map(x => x.Street).Index(3).Name("Straat");
            Map(x => x.HouseNumber).Index(4).Name("Huisnummer");
            Map(x => x.PostalCode).Index(5).Name("Postcode");
            Map(x => x.City).Index(6).Name("Plaats");
            Map(x => x.Country).Index(7).Name("Land");
            Map(x => x.PhoneNumber).Index(8).Name("Telefoon nummer");
            Map(x => x.Email).Index(9).Name("E-mail");
            Map(x => x.Timeframe).Index(10).Name("Tijdvenster");
            Map(x => x.Instructions).Index(11).Name("Instructies");
            Map(x => x.DeliveryDate).Index(12).Name("Leverdatum");
            Map(x => x.Customer).Index(13).Name("Klant");
            Map(x => x.Barcode).Index(14).Name("Barcode");
            Map(x => x.ColliType).Index(15).Name("Colli type");
        }
    }
}
