﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Configuration;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Constants;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.NonPriceElementModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Competitions.Models.NonPriceElementModels;

public static class NonPriceElementsModelTests
{
    [Theory]
    [CommonAutoData]
    public static void Construct_SetsPropertiesAsExpected(
        Organisation organisation,
        NonPriceElements elements,
        Competition competition)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = elements;

        var model = new NonPriceElementsModel(competition);

        model.InternalOrgId.Should().Be(organisation.InternalIdentifier);
        model.CompetitionId.Should().Be(competition.Id);
        model.CompetitionName.Should().Be(competition.Name);
        model.NonPriceElements.Should().BeEquivalentTo(elements);
    }

    [Theory]
    [CommonAutoData]
    public static void HasAllNonPriceElements_ReturnsExpected(
        Organisation organisation,
        Competition competition)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = new()
        {
            Implementation = new(),
            Interoperability = new List<InteroperabilityCriteria> { new() },
            ServiceLevel = new(),
        };

        var model = new NonPriceElementsModel(competition);

        model.HasAllNonPriceElements().Should().BeTrue();
    }

    [Theory]
    [CommonAutoData]
    public static void HasAllNonPriceElements_None_ReturnsExpected(
        Organisation organisation,
        Competition competition)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = new();

        var model = new NonPriceElementsModel(competition);

        model.HasAllNonPriceElements().Should().BeFalse();
    }

    [Theory]
    [CommonAutoData]
    public static void HasAnyNonPriceElements_ReturnsExpected(
        Organisation organisation,
        Competition competition)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = new()
        {
            Implementation = new(),
            Interoperability = new List<InteroperabilityCriteria> { new() },
            ServiceLevel = new(),
        };

        var model = new NonPriceElementsModel(competition);

        model.HasAnyNonPriceElements().Should().BeTrue();
    }

    [Theory]
    [CommonAutoData]
    public static void HasAnyNonPriceElements_None_ReturnsExpected(
        Organisation organisation,
        Competition competition)
    {
        competition.Organisation = organisation;
        competition.NonPriceElements = new();

        var model = new NonPriceElementsModel(competition);

        model.HasAnyNonPriceElements().Should().BeFalse();
    }

    [Theory]
    [CommonAutoData]
    public static void GetIm1Integrations_Returns(
        Organisation organisation,
        Competition competition)
    {
        var im1Integrations = Interoperability.Im1Integrations
            .Select(x => new InteroperabilityCriteria(x.Key, InteropIntegrationType.Im1))
            .ToList();

        competition.Organisation = organisation;
        competition.NonPriceElements = new() { Interoperability = im1Integrations };

        var model = new NonPriceElementsModel(competition);

        var actualIntegrations = model.GetIm1Integrations();

        actualIntegrations.Should().HaveCount(im1Integrations.Count);
        actualIntegrations
            .Should()
            .BeEquivalentTo(im1Integrations.Select(x => Interoperability.Im1Integrations[x.Qualifier]));
    }

    [Theory]
    [CommonAutoData]
    public static void GetGpConnectIntegrations_Returns(
        Organisation organisation,
        Competition competition)
    {
        var gpConnectIntegrations = Interoperability.GpConnectIntegrations
            .Select(x => new InteroperabilityCriteria(x.Key, InteropIntegrationType.GpConnect))
            .ToList();

        competition.Organisation = organisation;
        competition.NonPriceElements = new() { Interoperability = gpConnectIntegrations };

        var model = new NonPriceElementsModel(competition);

        var actualIntegrations = model.GetGpConnectIntegrations();

        actualIntegrations.Should().HaveCount(gpConnectIntegrations.Count);
        actualIntegrations
            .Should()
            .BeEquivalentTo(gpConnectIntegrations.Select(x => Interoperability.GpConnectIntegrations[x.Qualifier]));
    }
}
