﻿using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.SolutionSelection.Prices.Base;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Order.Models.SolutionSelection.Prices.Base
{
    public abstract class PricingModelTests
    {
        protected abstract Type ModelType { get; }

        [Theory]
        [CommonAutoData]
        public void WithValidCatalogueItem_PropertiesCorrectlySet(CatalogueItem item)
        {
            var price = item.CataloguePrices.First();

            var model = (PricingModel)Activator.CreateInstance(ModelType, item, price.CataloguePriceId, null);

            model!.Title.Should().Be(string.Format(PricingModel.TitleText, model.ItemType.Name()));
            model.Caption.Should().Be(model.ItemName);
            model.Basis.Should().Be(price.ToPriceUnitString());
            model.CalculationType.Should().Be(price.CataloguePriceCalculationType);
            model.ItemName.Should().Be(item.Name);
            model.ItemType.Should().Be(item.CatalogueItemType);
            model.NumberOfTiers.Should().Be(price.CataloguePriceTiers.Count);
            model.PriceType.Should().Be(price.CataloguePriceType);

            foreach (var tier in model.Tiers)
            {
                var pricingTier = price.CataloguePriceTiers.First(x => x.Id == tier.Id);

                tier.AgreedPrice.Should().Be(pricingTier.Price.ToString(PricingModel.FourDecimalPlaces, CultureInfo.InvariantCulture));
                tier.Description.Should().Be(pricingTier.GetRangeDescription());
                tier.ListPrice.Should().Be(pricingTier.Price);
                tier.LowerRange.Should().Be(pricingTier.LowerRange);
                tier.UpperRange.Should().Be(pricingTier.UpperRange);
            }
        }

        [Theory]
        [CommonAutoData]
        public void WithValidCatalogueItem_AndExistingOrderItem_PropertiesCorrectlySet(CatalogueItem item, OrderItem orderItem)
        {
            var price = item.CataloguePrices.First();

            orderItem.OrderItemPrice.CataloguePriceId = price.CataloguePriceId;
            orderItem.OrderItemPrice.OrderItemPriceTiers = price.CataloguePriceTiers
                .Select(x => new OrderItemPriceTier(orderItem.OrderItemPrice, x)
                {
                    Price = x.Price / 2,
                })
                .ToList();

            var model = (PricingModel)Activator.CreateInstance(ModelType, item, price.CataloguePriceId, orderItem);

            model!.Title.Should().Be(string.Format(PricingModel.TitleText, model.ItemType.Name()));
            model.Caption.Should().Be(model.ItemName);
            model.Basis.Should().Be(price.ToPriceUnitString());
            model.CalculationType.Should().Be(price.CataloguePriceCalculationType);
            model.ItemName.Should().Be(item.Name);
            model.ItemType.Should().Be(item.CatalogueItemType);
            model.NumberOfTiers.Should().Be(price.CataloguePriceTiers.Count);
            model.PriceType.Should().Be(price.CataloguePriceType);

            foreach (var tier in model.Tiers)
            {
                var pricingTier = price.CataloguePriceTiers.First(x => x.Id == tier.Id);
                var tierPrice = pricingTier.Price / 2;

                tier.AgreedPrice.Should().Be(tierPrice.ToString(PricingModel.FourDecimalPlaces, CultureInfo.InvariantCulture));
                tier.Description.Should().Be(pricingTier.GetRangeDescription());
                tier.ListPrice.Should().Be(pricingTier.Price);
                tier.LowerRange.Should().Be(pricingTier.LowerRange);
                tier.UpperRange.Should().Be(pricingTier.UpperRange);
            }
        }

        [Theory]
        [CommonAutoData]
        public void WithValidOrderItem_PropertiesCorrectlySet(OrderItem item)
        {
            var price = item.OrderItemPrice;

            var model = (PricingModel)Activator.CreateInstance(ModelType, item);

            model!.Title.Should().Be(string.Format(PricingModel.TitleText, model.ItemType.Name()));
            model.Caption.Should().Be(model.ItemName);
            model.Basis.Should().Be(price.ToPriceUnitString());
            model.CalculationType.Should().Be(price.CataloguePriceCalculationType);
            model.ItemName.Should().Be(item.CatalogueItem.Name);
            model.ItemType.Should().Be(item.CatalogueItem.CatalogueItemType);
            model.NumberOfTiers.Should().Be(price.OrderItemPriceTiers.Count);
            model.PriceType.Should().Be(price.CataloguePriceType);

            foreach (var tier in model.Tiers)
            {
                var pricingTier = price.OrderItemPriceTiers.First(x => x.Id == tier.Id);

                tier.AgreedPrice.Should().Be(pricingTier.Price.ToString(PricingModel.FourDecimalPlaces, CultureInfo.InvariantCulture));
                tier.Description.Should().Be(pricingTier.GetRangeDescription());
                tier.ListPrice.Should().Be(pricingTier.ListPrice);
                tier.LowerRange.Should().Be(pricingTier.LowerRange);
                tier.UpperRange.Should().Be(pricingTier.UpperRange);
            }
        }
    }
}
