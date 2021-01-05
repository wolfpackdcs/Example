using Orp.Core.Orders.DTO.Orders.V2;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Extensions;
using Shouldly;
using System;
using Xunit;

namespace Orp.Core.Orders.Sync.RoutiGo.UnitTests
{
    public class ShipmentDTOExtensionMethodsTests
    {
        [Theory]
        [InlineData("NL", "1031HL", true)]
        [InlineData("NL", "2031HL", true)]
        [InlineData("NL", "3031HL", false)]
        [InlineData("BE", "1031", false)]
        [InlineData("BE", "2031", false)]
        public void XenosFilter_OnlyNLAndPrefixedWith1(string countryCode, string postalCode, bool expected)
        {
            //Arrange.
            ShipmentDTO shipment = new ShipmentDTO()
            {
                ShippingAddress = new AddressDTO()
                {
                    CountryCode = countryCode,
                    PostalCode = postalCode
                }
            };

            Environment.SetEnvironmentVariable("XENOS_POSTALCODE_PREFIXES", "1;2");

            //Act.
            bool actual = shipment.CanBeExported();

            //Assert.
            actual.ShouldBe(expected);
        }
    }
}
