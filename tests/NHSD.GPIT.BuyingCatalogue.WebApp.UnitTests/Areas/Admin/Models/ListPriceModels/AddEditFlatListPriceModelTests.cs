﻿using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ListPriceModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Models.ListPriceModels;

public static class AddEditFlatListPriceModelTests
{
    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void Construct_OnDemandPrice_AssignsBillingPeriod(
        TimeUnit? billingPeriod,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.OnDemand;
        price.TimeUnit = billingPeriod;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandBillingPeriod.Should().Be(billingPeriod);
        model.DeclarativeBillingPeriod.Should().BeNull();
        model.PerServiceRecipientBillingPeriod.Should().BeNull();
    }

    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void Construct_DeclarativePrice_AssignsBillingPeriod(
        TimeUnit? billingPeriod,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.Declarative;
        price.TimeUnit = billingPeriod;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandBillingPeriod.Should().BeNull();
        model.DeclarativeBillingPeriod.Should().Be(billingPeriod);
        model.PerServiceRecipientBillingPeriod.Should().BeNull();
    }

    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void Construct_PerServiceRecipientPrice_AssignsBillingPeriod(
        TimeUnit? billingPeriod,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.PerServiceRecipient;
        price.TimeUnit = billingPeriod;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandBillingPeriod.Should().BeNull();
        model.DeclarativeBillingPeriod.Should().BeNull();
        model.PerServiceRecipientBillingPeriod.Should().Be(billingPeriod);
    }

    [Theory]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerServiceRecipient)]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerSolutionOrService)]
    public static void Construct_OnDemandPrice_AssignsQuantityCalculationType(
        CataloguePriceQuantityCalculationType calculationType,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.OnDemand;
        price.CataloguePriceQuantityCalculationType = calculationType;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandQuantityCalculationType.Should().Be(calculationType);
        model.DeclarativeQuantityCalculationType.Should().BeNull();
    }

    [Theory]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerServiceRecipient)]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerSolutionOrService)]
    public static void Construct_DeclarativePrice_AssignsQuantityCalculationType(
        CataloguePriceQuantityCalculationType calculationType,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.Declarative;
        price.CataloguePriceQuantityCalculationType = calculationType;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandQuantityCalculationType.Should().BeNull();
        model.DeclarativeQuantityCalculationType.Should().Be(calculationType);
    }

    [Theory]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerServiceRecipient)]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerSolutionOrService)]
    public static void Construct_NullQuantityCalculationType(
        CataloguePriceQuantityCalculationType calculationType,
        CatalogueItem catalogueItem,
        CataloguePrice price)
    {
        price.ProvisioningType = ProvisioningType.PerServiceRecipient;
        price.CataloguePriceQuantityCalculationType = calculationType;

        var model = new AddEditFlatListPriceModel(catalogueItem, price);

        model.OnDemandQuantityCalculationType.Should().BeNull();
        model.DeclarativeQuantityCalculationType.Should().BeNull();
    }

    [Theory]
    [CommonAutoData]
    public static void GetBillingPeriod_Patient_PerYear(
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.Patient;

        model.GetBillingPeriod().Should().Be(TimeUnit.PerYear);
    }

    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void GetBillingPeriod_PerServiceRecipient_SelectedTimeUnit(
        TimeUnit? selectedBillingPeriod,
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.PerServiceRecipient;
        model.PerServiceRecipientBillingPeriod = selectedBillingPeriod;

        model.GetBillingPeriod().Should().Be(selectedBillingPeriod);
    }

    [Theory]
    [CommonAutoData]
    public static void GetBillingPeriod_DeclarativeAssociatedService_Null(
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.Declarative;
        model.CatalogueItemType = CatalogueItemType.AssociatedService;

        model.GetBillingPeriod().Should().BeNull();
    }

    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void GetBillingPeriod_Declarative_SelectedTimeUnit(
        TimeUnit? selectedBillingPeriod,
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.Declarative;
        model.DeclarativeBillingPeriod = selectedBillingPeriod;
        model.CatalogueItemType = CatalogueItemType.AdditionalService;

        model.GetBillingPeriod().Should().Be(selectedBillingPeriod);
    }

    [Theory]
    [CommonInlineAutoData(null)]
    [CommonInlineAutoData(TimeUnit.PerMonth)]
    [CommonInlineAutoData(TimeUnit.PerYear)]
    public static void GetBillingPeriod_OnDemand_SelectedTimeUnit(
        TimeUnit? selectedBillingPeriod,
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.OnDemand;
        model.OnDemandBillingPeriod = selectedBillingPeriod;
        model.CatalogueItemType = CatalogueItemType.AdditionalService;

        model.GetBillingPeriod().Should().Be(selectedBillingPeriod);
    }

    [Theory]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerServiceRecipient)]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerSolutionOrService)]
    public static void GetQuantityCalculationType_Declarative_SelectedQuantityCalculationType(
        CataloguePriceQuantityCalculationType quantityCalculationType,
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.Declarative;
        model.DeclarativeQuantityCalculationType = quantityCalculationType;
        model.OnDemandQuantityCalculationType = null;

        model.GetQuantityCalculationType().Should().Be(quantityCalculationType);
    }

    [Theory]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerServiceRecipient)]
    [CommonInlineAutoData(CataloguePriceQuantityCalculationType.PerSolutionOrService)]
    public static void GetQuantityCalculationType_OnDemand_SelectedQuantityCalculationType(
        CataloguePriceQuantityCalculationType quantityCalculationType,
        AddEditFlatListPriceModel model)
    {
        model.SelectedProvisioningType = ProvisioningType.OnDemand;
        model.DeclarativeQuantityCalculationType = null;
        model.OnDemandQuantityCalculationType = quantityCalculationType;

        model.GetQuantityCalculationType().Should().Be(quantityCalculationType);
    }
}
