﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Objects.Marketing;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Marketing.Hosting
{
    public sealed class OnPremise : TestBase, IClassFixture<LocalWebApplicationFactory>, IDisposable
    {
        public OnPremise(LocalWebApplicationFactory factory) : base(factory, "marketing/supplier/solution/99999-99/section/hosting-type-on-premise")
        {
        }

        [Fact]
        public async Task OnPremise_CompleteAllFields()
        {
            var summary = TextGenerators.TextInputAddText(HostingTypesObjects.OnPremise_Summary, 500);
            var link = TextGenerators.UrlInputAddText(HostingTypesObjects.OnPremise_Link, 1000);
            var hostingModel = TextGenerators.TextInputAddText(HostingTypesObjects.OnPremise_HostingModel, 1000);
            var expected = new ServiceContracts.Solutions.OnPremise
            {
                HostingModel = hostingModel,
                Link = link,
                Summary = summary,
            };

            MarketingPages.HostingTypeActions.ToggleHSCNCheckbox();
            CommonActions.ClickSave();

            using var context = GetBCContext();
            var hosting = (await context.Solutions.SingleAsync(s => s.Id == "99999-99")).Hosting;
            
            var actual = JsonConvert.DeserializeObject<ServiceContracts.Solutions.Hosting>(hosting);
            actual.OnPremise.Should().BeEquivalentTo(expected, opt => opt.Excluding(o => o.RequiresHscn));
        }

        [Fact]
        public void OnPremise_SectionComplete()
        {
            TextGenerators.TextInputAddText(HostingTypesObjects.OnPremise_Summary, 500);
            TextGenerators.UrlInputAddText(HostingTypesObjects.OnPremise_Link, 1000);
            TextGenerators.TextInputAddText(HostingTypesObjects.OnPremise_HostingModel, 1000);
            MarketingPages.HostingTypeActions.ToggleHSCNCheckbox();

            CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("On premise").Should().BeTrue();
        }

        [Fact]
        public void OnPremise_SectionIncomplete()
        {
            CommonActions.ClickGoBackLink();

            MarketingPages.DashboardActions.SectionMarkedComplete("On premise").Should().BeFalse();
        }

        public void Dispose()
        {
            ClearHostingTypes("99999-99");
        }
    }
}