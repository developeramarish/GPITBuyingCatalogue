﻿using System;
using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Marketing.Models.NativeDesktop;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Marketing.Models.NativeDesktop
{
    public static class ThirdPartyModelTests
    {
        [Fact]
        public static void Constructor_NullCatalogueItem_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _ = new ThirdPartyModel(null));
        }

        [Fact]
        public static void WithCatalogueItem_PropertiesCorrectlySet()
        {
            var clientApplication = new ClientApplication
            {
                NativeDesktopThirdParty = new NativeDesktopThirdParty { ThirdPartyComponents = "Third party components", DeviceCapabilities = "Device capabilities" },
            };
            var json = JsonConvert.SerializeObject(clientApplication);
            var catalogueItem = new CatalogueItem
            {
                Id = new CatalogueItemId(1, "123"),
                Solution = new Solution { ClientApplication = json },
            };

            var model = new ThirdPartyModel(catalogueItem);

            Assert.Equal("/marketing/supplier/solution/1-123/section/native-desktop", model.BackLink);
            Assert.Equal("Third party components", model.ThirdPartyComponents);
            Assert.Equal("Device capabilities", model.DeviceCapabilities);
        }

        [Fact]
        public static void WithoutCatalogueItem_PropertiesAreDefaulted()
        {
            var model = new ThirdPartyModel();

            Assert.Equal("./", model.BackLink);
            Assert.False(model.IsComplete);
            Assert.Null(model.ThirdPartyComponents);
            Assert.Null(model.DeviceCapabilities);
        }

        [Theory]
        [InlineData(null, null, false)]
        [InlineData("", null, false)]
        [InlineData(" ", null, false)]
        [InlineData(null, "", false)]
        [InlineData(null, " ", false)]
        [InlineData("Third party", null, true)]
        [InlineData(null, "Device capability", true)]
        public static void IsCompleteIsCorrectlySet(string thirdParty, string deviceCapabilities, bool? expected)
        {
            var clientApplication = new ClientApplication
            {
                NativeDesktopThirdParty = new NativeDesktopThirdParty { ThirdPartyComponents = thirdParty, DeviceCapabilities = deviceCapabilities },
            };
            var json = JsonConvert.SerializeObject(clientApplication);
            var catalogueItem = new CatalogueItem { Solution = new Solution { ClientApplication = json } };

            var model = new ThirdPartyModel(catalogueItem);

            Assert.Equal(expected, model.IsComplete);
        }
    }
}
