﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Marketing;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Marketing.ClientApplication.NativeDesktop
{
    public sealed class SupportedOperatingSystems : TestBase, IClassFixture<LocalWebApplicationFactory>, IDisposable
    {
        public SupportedOperatingSystems(LocalWebApplicationFactory factory)
            : base(factory, "marketing/supplier/solution/99999-002/section/native-desktop/operating-systems")
        {
            AuthorityLogin();
        }

        [Fact]
        public async Task SupportedOperatingSystems_CompleteAllFields()
        {
            var operatingSystems = TextGenerators.TextInputAddText(CommonSelectors.SupportedOperatingSystemDescription, 1000);

            CommonActions.ClickSave();

            await using var context = GetEndToEndDbContext();

            var clientApplication = (await context.Solutions.SingleAsync(s => s.CatalogueItemId == new CatalogueItemId(99999, "002"))).ClientApplication;
            clientApplication.Should().ContainEquivalentOf($"\"NativeDesktopOperatingSystemsDescription\":\"{operatingSystems}\"");
        }

        [Fact]
        public void SupportedOperatingSystems_SectionComplete()
        {
            TextGenerators.TextInputAddText(CommonSelectors.SupportedOperatingSystemDescription, 1000);

            CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("Supported operating systems").Should().BeTrue();
        }

        [Fact]
        public void SupportedOperatingSystems_SectionIncomplete()
        {
            CommonActions.ClickGoBackLink();

            MarketingPages.DashboardActions.SectionMarkedComplete("Supported operating systems").Should().BeFalse();
        }

        public void Dispose()
        {
            ClearClientApplication(new CatalogueItemId(99999, "002"));
        }
    }
}
