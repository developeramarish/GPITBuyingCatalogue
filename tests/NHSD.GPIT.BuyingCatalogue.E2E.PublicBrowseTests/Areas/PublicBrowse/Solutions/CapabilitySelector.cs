﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils.TestBases;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.PublicBrowse.Solutions
{
    public sealed class CapabilitySelector : TestBase, IClassFixture<LocalWebApplicationFactory>
    {
        public CapabilitySelector(LocalWebApplicationFactory factory)
            : base(factory, "solutions/futures/capabilitiesselector")
        {
        }

        [Fact]
        public async Task CapabilitySelector_AllCapabilitiesDisplayed()
        {
            await using var context = GetEndToEndDbContext();
            var capabilities = await context.FrameworkCapabilities.Include(s => s.Capability).Where(s => s.FrameworkId == "NHSDGP001").ToListAsync();

            var capabilityCheckboxes = PublicBrowsePages.CapabilitySelectorActions.GetAllCheckboxLabels();

            capabilities.Select(c => c.Capability.Name).Should().BeEquivalentTo(capabilityCheckboxes);
        }

        [Fact]
        public void CapabilitySelector_GoBack()
        {
            CommonActions.ClickGoBackLink();

            CommonActions.PageTitle()
                .Should()
                .ContainEquivalentOf("GP IT Futures framework".FormatForComparison());
        }

        [Fact]
        public async Task CapabilitySelector_NoCheckboxSelected()
        {
            CommonActions.ClickSave();

            await using var context = GetEndToEndDbContext();
            var solutionsCount =
                await context.CatalogueItems
                .Include(c => c.Solution).ThenInclude(s => s.FrameworkSolutions)
                .Where(c =>
                    c.PublishedStatus == EntityFramework.Catalogue.Models.PublicationStatus.Published
                    && c.CatalogueItemType == EntityFramework.Catalogue.Models.CatalogueItemType.Solution
                    && c.Solution.FrameworkSolutions.Any(fs => fs.FrameworkId == "NHSDGP001"))
                .CountAsync();

            PublicBrowsePages.SolutionsActions.GetSolutionsInList().Should().HaveCount(solutionsCount);
        }

        [Fact]
        public void CapabilitySelector_SingleCheckboxSelected()
        {
            PublicBrowsePages.CapabilitySelectorActions.ClickFirstCheckbox();

            CommonActions.ClickSave();

            PublicBrowsePages.SolutionsActions.GetSolutionsInList().Should().HaveCount(1);
        }
    }
}
