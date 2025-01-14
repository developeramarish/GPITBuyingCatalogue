﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.CapabilityModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.CapabilityModels;

public static class CapabilityModelTests
{
    [Theory]
    [CommonAutoData]
    public static void Construct_SetsProperties(
        Solution solution,
        Capability capability)
    {
        var model = new CapabilityModel(solution.CatalogueItem, capability);

        model.Id.Should().Be(capability.Id);
        model.Name.Should().Be(capability.Name);
        model.CapabilityRef.Should().Be(capability.CapabilityRef);
        model.Selected.Should().BeFalse();
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_Selected_SetsSelected(
        Solution solution,
        Capability capability)
    {
        solution.CatalogueItem.CatalogueItemCapabilities.Add(new(solution.CatalogueItemId, capability.Id));

        var model = new CapabilityModel(solution.CatalogueItem, capability);

        model.Selected.Should().BeTrue();
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_Epics_SplitsMustAndMay(
        Solution solution,
        Capability capability,
        List<Epic> mustEpics,
        List<Epic> mayEpics)
    {
        mustEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.Must);
        mayEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.May);

        capability.Epics = mustEpics.Concat(mayEpics).ToList();
        solution.CatalogueItem.CatalogueItemCapabilities.Add(new(solution.CatalogueItemId, capability.Id));

        var model = new CapabilityModel(solution.CatalogueItem, capability);

        model.MustEpics.Should().HaveCount(mustEpics.Count);
        model.MayEpics.Should().HaveCount(mayEpics.Count);
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_CapabilityNotSelected_SelectsAllMustEpics(
        Solution solution,
        Capability capability,
        List<Epic> mustEpics,
        List<Epic> mayEpics)
    {
        mustEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.Must);
        mayEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.May);

        capability.Epics = mustEpics.Concat(mayEpics).ToList();
        solution.CatalogueItem.CatalogueItemCapabilities = Enumerable.Empty<CatalogueItemCapability>().ToList();

        var model = new CapabilityModel(solution.CatalogueItem, capability);

        model.MustEpics.Should().OnlyContain(e => e.Selected == true);
        model.MustEpics.Should().HaveCount(mustEpics.Count);
        model.MayEpics.Should().HaveCount(mayEpics.Count);
    }

    [Theory]
    [CommonAutoData]
    public static void Construct_CapabilitySelected_PreservesMustEpics(
        Solution solution,
        Capability capability,
        List<Epic> mustEpics,
        List<Epic> mayEpics)
    {
        mustEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.Must);
        mayEpics.ForEach(e => e.CompliancyLevel = CompliancyLevel.May);

        capability.Epics = mustEpics.Concat(mayEpics).ToList();
        solution.CatalogueItem.CatalogueItemCapabilities.Add(new(solution.CatalogueItemId, capability.Id));
        solution.CatalogueItem.CatalogueItemEpics = new List<CatalogueItemEpic>
        {
            new(solution.CatalogueItemId, capability.Id, mustEpics.First().Id),
        };

        var model = new CapabilityModel(solution.CatalogueItem, capability);

        model.MustEpics.Should().ContainSingle(e => e.Selected == true);
        model.MustEpics.Should().HaveCount(mustEpics.Count);
        model.MayEpics.Should().HaveCount(mayEpics.Count);
    }
}
