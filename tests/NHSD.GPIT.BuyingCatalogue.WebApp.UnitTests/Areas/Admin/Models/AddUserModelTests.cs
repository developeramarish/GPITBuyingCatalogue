﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models
{
    public static class AddUserModelTests
    {
        [Theory]
        [CommonAutoData]
        public static void WithValidConstruction_PropertiesSetAsExpected(
            Organisation organisation)
        {
            var actual = new AddUserModel(organisation);

            actual.BackLinkText.Should().Be("Go back to previous page");
            actual.BackLink.Should().Be($"/admin/organisations/{organisation.Id}");
            actual.Organisation.Should().Be(organisation);
        }
    }
}
