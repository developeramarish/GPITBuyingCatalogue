﻿using System;
using System.Linq;
using AutoFixture;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;

namespace NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations
{
    internal sealed class SupplierCustomization : ICustomization
    {
        private static readonly Random Random = new();

        public void Customize(IFixture fixture)
        {
            fixture.Customize<Supplier>(
                c => c.With(
                    s => s.CatalogueItems,
                    Enumerable.Range(1, Random.Next(2, 6))
                        .ToList()
                        .Select(
                            i => new CatalogueItem
                            {
                                AssociatedService = fixture.Create<AssociatedService>(),
                                CataloguePrices = fixture.CreateMany<CataloguePrice>().ToList(),
                            })
                        .ToList));
        }
    }
}