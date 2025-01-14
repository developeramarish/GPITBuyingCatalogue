﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.CapabilityModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.CapabilityModels;

public static class EditCapabilitiesModelTests
{
    [Theory]
    [CommonAutoData]
    public static void Construct_Solution_SetsProperties(
        Solution solution,
        List<CapabilityCategory> capabilityCategories)
    {
        var model = new EditCapabilitiesModel(solution.CatalogueItem, capabilityCategories);

        model.CatalogueItemType.Should().Be(solution.CatalogueItem.CatalogueItemType.Name());
        model.Title.Should().Be("Capabilities and Epics");
        model.CapabilityCategories.Should().HaveCount(capabilityCategories.Count(cc => cc.Capabilities.Any()));
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_Service_SetsProperties(
        AdditionalService additionalService,
        List<CapabilityCategory> capabilityCategories)
    {
        var model = new EditCapabilitiesModel(additionalService.CatalogueItem, capabilityCategories);

        model.CatalogueItemType.Should().Be(additionalService.CatalogueItem.CatalogueItemType.Name());
        model.Title.Should().Be($"{additionalService.CatalogueItem.Name} Capabilities and Epics");
        model.CapabilityCategories.Should().HaveCount(capabilityCategories.Count(cc => cc.Capabilities.Any()));
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_NoSelectedCapabilities_SelectsAllMustEpics(
        Solution solution,
        CapabilityCategory capabilityCategory,
        Capability capability,
        List<Epic> mustEpics,
        List<Epic> mayEpics,
        List<CapabilityCategory> capabilityCategories)
    {
        mustEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.Must);
        mayEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.May);

        capability.Epics = mustEpics.Concat(mayEpics).ToList();

        capabilityCategory.Capabilities.Add(capability);
        capabilityCategories.Add(capabilityCategory);

        solution.CatalogueItem.CatalogueItemCapabilities = Enumerable.Empty<CatalogueItemCapability>().ToList();

        var model = new EditCapabilitiesModel(solution.CatalogueItem, capabilityCategories);

        var allCapabilities = model.CapabilityCategories.SelectMany(cc => cc.Capabilities).ToList();

        allCapabilities.SelectMany(c => c.MustEpics).Should().OnlyContain(e => e.Selected == true);
        allCapabilities.SelectMany(c => c.MayEpics).Should().OnlyContain(e => e.Selected == false);
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_SelectedCapabilities_DoesNotSelectAllMustEpics(
        Solution solution,
        CapabilityCategory capabilityCategory,
        Capability capability,
        List<Epic> mustEpics,
        List<Epic> mayEpics,
        List<CapabilityCategory> capabilityCategories)
    {
        mustEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.Must);
        mayEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.May);

        capability.Epics = mustEpics.Concat(mayEpics).ToList();

        capabilityCategory.Capabilities.Add(capability);
        capabilityCategories.Add(capabilityCategory);

        solution.CatalogueItem.CatalogueItemCapabilities.Add(new(solution.CatalogueItemId, capability.Id));

        var model = new EditCapabilitiesModel(solution.CatalogueItem, capabilityCategories);

        var allCapabilities = model.CapabilityCategories.SelectMany(cc => cc.Capabilities).ToList();

        allCapabilities.SelectMany(c => c.Epics).Should().OnlyContain(e => e.Selected == false);
    }
}
