﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Competitions.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Framework.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Competitions;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Models.NonPriceElementModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Competitions.Models.NonPriceElementModels;

public static class AddNonPriceElementModelTests
{
    public static IEnumerable<object[]> AddNonPriceElementsTestData => new[]
    {
        new object[]
        {
            new NonPriceElements(),
            new List<SelectOption<NonPriceElement>>
            {
                new(NonPriceElement.Implementation.EnumMemberName(), NonPriceElement.Implementation),
                new(NonPriceElement.Interoperability.EnumMemberName(), NonPriceElement.Interoperability),
                new(NonPriceElement.ServiceLevel.EnumMemberName(), NonPriceElement.ServiceLevel),
            },
        },
        new object[]
        {
            new NonPriceElements
            {
                ServiceLevel = new(),
            },
            new List<SelectOption<NonPriceElement>>
            {
                new(NonPriceElement.Implementation.EnumMemberName(), NonPriceElement.Implementation),
                new(NonPriceElement.Interoperability.EnumMemberName(), NonPriceElement.Interoperability),
            },
        },
        new object[]
        {
            new NonPriceElements
            {
                Interoperability = new List<InteroperabilityCriteria>
                {
                    new(),
                },
            },
            new List<SelectOption<NonPriceElement>>
            {
                new(NonPriceElement.Implementation.EnumMemberName(), NonPriceElement.Implementation),
                new(NonPriceElement.ServiceLevel.EnumMemberName(), NonPriceElement.ServiceLevel),
            },
        },
        new object[]
        {
            new NonPriceElements
            {
                Implementation = new(),
            },
            new List<SelectOption<NonPriceElement>>
            {
                new(NonPriceElement.ServiceLevel.EnumMemberName(), NonPriceElement.ServiceLevel),
                new(NonPriceElement.Interoperability.EnumMemberName(), NonPriceElement.Interoperability),
            },
        },
        new object[]
        {
            new NonPriceElements
            {
                Implementation = new(),
                ServiceLevel = new(),
                Interoperability = new List<InteroperabilityCriteria>
                {
                    new(),
                },
            },
            Enumerable.Empty<SelectOption<NonPriceElement>>(),
        },
    };

    [Theory]
    [CommonMemberAutoData(nameof(AddNonPriceElementsTestData))]
    public static void Construct_SetsPropertiesAsExpected(
        NonPriceElements elements,
        IEnumerable<SelectOption<NonPriceElement>> expected,
        Competition competition)
    {
        competition.NonPriceElements = elements;

        var model = new AddNonPriceElementModel(competition);

        model.CompetitionName.Should().Be(competition.Name);
        model.AvailableNonPriceElements.Should().BeEquivalentTo(expected);
    }
}
