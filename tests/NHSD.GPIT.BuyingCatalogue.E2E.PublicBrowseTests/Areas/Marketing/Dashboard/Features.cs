﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Marketing.Dashboard
{
    public sealed class Features : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public Features(LocalWebApplicationFactory factory) : base(factory, "marketing/supplier/solution/99999-99/section/features")
        {
            using var context = GetBCContext();
            var solution = context.Solutions.Single(s => s.Id == "99999-99");
            solution.Features = string.Empty;
            context.SaveChanges();
        }

        [Fact]
        public async Task Features_AddFeaturesAsync()
        {
            var feature = MarketingPages.FeaturesActions.EnterFeature();
            MarketingPages.CommonActions.ClickSave();

            using var context = GetBCContext();
            var solution = await context.Solutions.SingleAsync(s => s.Id == "99999-99");

            solution.Features.Should().ContainEquivalentOf(feature);
        }

        [Fact]
        public async Task Features_Add10FeaturesAsync()
        {
            List<string> addedFeatures = new();

            for (int i = 0; i < 10; i++)
            {
                var feature = MarketingPages.FeaturesActions.EnterFeature(i);
                addedFeatures.Add(feature);
            }

            MarketingPages.CommonActions.ClickSave();

            using var context = GetBCContext();
            var solution = await context.Solutions.SingleAsync(s => s.Id == "99999-99");

            foreach (var feature in addedFeatures)
            {
                solution.Features.Should().ContainEquivalentOf(feature);
            }
        }

        [Fact]
        public void Features_SectionMarkedComplete()
        {
            driver.Navigate().Refresh();

            var feature = MarketingPages.FeaturesActions.EnterFeature();
            MarketingPages.CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("Features").Should().BeTrue();
        }

        [Fact]
        public void Features_SectionMarkedIncomplete()
        {
            driver.Navigate().Refresh();

            MarketingPages.CommonActions.ClickSave();

            MarketingPages.DashboardActions.SectionMarkedComplete("Features").Should().BeFalse();
        }
    }
}