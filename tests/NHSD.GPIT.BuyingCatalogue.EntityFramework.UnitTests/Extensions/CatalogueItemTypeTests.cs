﻿using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Extensions;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.UnitTests.Extensions
{
    public static class CatalogueItemTypeTests
    {
        [Theory]
        [InlineData(CatalogueItemType.AdditionalService, "Additional Service")]
        [InlineData(CatalogueItemType.AssociatedService, "Associated Service")]
        [InlineData(CatalogueItemType.Solution, "Catalogue Solution")]
        public static void DisplayName_ReturnsExpectedName(CatalogueItemType itemType, string expectedName)
        {
            var displayName = itemType.DisplayName();

            displayName.Should().Be(expectedName);
        }

        [Theory]
        [MemberData(nameof(InferEstimationPeriodTestData.TestData), MemberType = typeof(InferEstimationPeriodTestData))]
        public static void InferEstimationPeriod_ReturnsExpectedEstimationPeriod(
            CatalogueItemType catalogueItemType,
            ProvisioningType provisioningType,
            TimeUnit? estimationPeriod,
            TimeUnit? expectedEstimationPeriod)
        {
            var actual = catalogueItemType.InferEstimationPeriod(provisioningType, estimationPeriod);

            actual.Should().Be(expectedEstimationPeriod);
        }

        [Theory]
        [AutoData]
        public static void InferEstimationPeriod_InvalidCatalogueItemType_ThrowsException(TimeUnit timeUnit)
        {
            const CatalogueItemType itemType = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => itemType.InferEstimationPeriod(0, timeUnit));
        }

        private static class InferEstimationPeriodTestData
        {
            public static IEnumerable<object[]> TestData()
            {
                foreach (var data in AdditionalServiceSolutionData(CatalogueItemType.AdditionalService))
                    yield return data;

                foreach (var data in AdditionalServiceSolutionData(CatalogueItemType.Solution))
                    yield return data;

                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.OnDemand, null, null };
                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.OnDemand, TimeUnit.PerYear, TimeUnit.PerYear };
                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.OnDemand, TimeUnit.PerMonth, TimeUnit.PerMonth };
                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.Declarative, null, null };
                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.Declarative, TimeUnit.PerYear, null };
                yield return new object[] { CatalogueItemType.AssociatedService, ProvisioningType.Declarative, TimeUnit.PerMonth, null };
            }

            private static IEnumerable<object[]> AdditionalServiceSolutionData(CatalogueItemType itemType)
            {
                yield return new object[] { itemType, ProvisioningType.OnDemand, null, null };
                yield return new object[] { itemType, ProvisioningType.OnDemand, TimeUnit.PerMonth, TimeUnit.PerMonth };
                yield return new object[] { itemType, ProvisioningType.OnDemand, TimeUnit.PerYear, TimeUnit.PerYear };

                yield return new object[] { itemType, ProvisioningType.Patient, null, TimeUnit.PerMonth };
                yield return new object[] { itemType, ProvisioningType.Patient, TimeUnit.PerMonth, TimeUnit.PerMonth };
                yield return new object[] { itemType, ProvisioningType.Patient, TimeUnit.PerYear, TimeUnit.PerMonth };

                yield return new object[] { itemType, ProvisioningType.Declarative, null, TimeUnit.PerYear };
                yield return new object[] { itemType, ProvisioningType.Declarative, TimeUnit.PerMonth, TimeUnit.PerYear };
                yield return new object[] { itemType, ProvisioningType.Declarative, TimeUnit.PerYear, TimeUnit.PerYear };
            }
        }
    }
}
