using Orp.Core.Orders.DTO.Orders.V2;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Mapping;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Models;
using Shouldly;
using Xunit;

namespace Orp.Core.Orders.Sync.RoutiGo.UnitTests
{
    public class ToDeliveryCSVMapperTests
    {
        [Fact]
        public void Map()
        {
            //Arrange.
            ToDeliveryCSVMapper mapper = new ToDeliveryCSVMapper();

            AddressDTO shippingAddress = new AddressDTO()
            {
                Address1 = "Asterweg",
                Address2 = "19",
                Address3 = "5B",
                LastName = "Henkie",
                PostalCode = "1031HL",
                City = "Amsterdam",
                Phone = "0612123456",
                Email = "gek@gestoord.com"
            };

            //Act.
            DeliveryCSV actual = mapper.Map("ORDER-1234", shippingAddress, "Trinitrotolueen");

            //Assert.
            actual.NrOfColli.ShouldBe("1");
            actual.OrderNumber.ShouldBe("ORDER-1234");
            actual.CustomerName.ShouldBe("Henkie");
            actual.Street.ShouldBe("Asterweg");
            actual.HouseNumber.ShouldBe("19 5B");
            actual.PostalCode.ShouldBe("1031HL");
            actual.City.ShouldBe("Amsterdam");
            actual.Country.ShouldBe("NL");
            actual.PhoneNumber.ShouldBe("0612123456");
            actual.Email.ShouldBe("gek@gestoord.com");
            actual.Timeframe.ShouldBeEmpty();
            actual.Instructions.ShouldBe("geen bijzonderheden");
            actual.DeliveryDate.ShouldBeEmpty();
            actual.Customer.ShouldBe("Xenos");
            actual.Barcode.ShouldBe("Trinitrotolueen");
            actual.ColliType.ShouldBe("Colli");
        }

        [Theory]
        [InlineData(null, null, "")]
        [InlineData("19", null, "19")]
        [InlineData(null, "19", "19")]
        [InlineData("19", "", "19")]
        [InlineData("19", "5B", "19 5B")]
        public void MapToHouseNumber_TestCombinations_ReturnsCorrectValue(string address2, string address3, string expected)
        {
            //Arrange.
            ToDeliveryCSVMapper mapper = new ToDeliveryCSVMapper();

            //Act.
            string actual = mapper.MapToHouseNumber(address2, address3);

            //Assert.
            actual.ShouldBe(expected);
        }
    }
}
