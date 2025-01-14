﻿using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Services.Contracts;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.Services.UnitTests.Contracts
{
    public class ContractsServiceTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(ContractsService).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetContract_ContractExists_ReturnsContract(
            int orderId,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            var flags = new ContractFlags { OrderId = orderId };

            dbContext.ContractFlags.Add(flags);

            await dbContext.SaveChangesAsync();

            var output = await service.GetContract(orderId);

            output.Should().Be(flags);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetContract_ContractDoesNotExist_CreatesNewContract(
            int orderId,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            var existing = await dbContext.ContractFlags.FirstOrDefaultAsync(x => x.OrderId == orderId);

            existing.Should().BeNull();

            var output = await service.GetContract(orderId);

            output.OrderId.Should().Be(orderId);

            var actual = await dbContext.ContractFlags.FirstOrDefaultAsync(x => x.OrderId == orderId);

            actual.Should().Be(output);
        }

        [Theory]
        [InMemoryDbInlineAutoData(true)]
        [InMemoryDbInlineAutoData(false)]
        public static async Task HasSpecificRequirements_UpdatesValue(
            bool value,
            int orderId,
            ContractFlags contractFlags,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            contractFlags.Order = null;
            contractFlags.OrderId = orderId;
            contractFlags.HasSpecificRequirements = null;

            dbContext.ContractFlags.Add(contractFlags);

            await dbContext.SaveChangesAsync();

            await service.HasSpecificRequirements(orderId, value);

            var actual = await dbContext.ContractFlags.FirstAsync(x => x.Id == contractFlags.Id);

            actual.HasSpecificRequirements.Should().Be(value);
        }

        [Theory]
        [InMemoryDbInlineAutoData(true)]
        [InMemoryDbInlineAutoData(false)]
        public static async Task UseDefaultBilling_UpdatesValue(
            bool value,
            int orderId,
            ContractFlags contractFlags,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            contractFlags.Order = null;
            contractFlags.OrderId = orderId;
            contractFlags.UseDefaultBilling = null;

            dbContext.ContractFlags.Add(contractFlags);

            await dbContext.SaveChangesAsync();

            await service.UseDefaultBilling(orderId, value);

            var actual = await dbContext.ContractFlags.FirstAsync(x => x.Id == contractFlags.Id);

            actual.UseDefaultBilling.Should().Be(value);
        }

        [Theory]
        [InMemoryDbInlineAutoData(true)]
        [InMemoryDbInlineAutoData(false)]
        public static async Task UseDefaultDataProcessing_UpdatesValue(
            bool value,
            int orderId,
            ContractFlags contractFlags,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            contractFlags.Order = null;
            contractFlags.OrderId = orderId;
            contractFlags.UseDefaultDataProcessing = null;

            dbContext.ContractFlags.Add(contractFlags);

            await dbContext.SaveChangesAsync();

            await service.UseDefaultDataProcessing(orderId, value);

            var actual = await dbContext.ContractFlags.FirstAsync(x => x.Id == contractFlags.Id);

            actual.UseDefaultDataProcessing.Should().Be(value);
        }

        [Theory]
        [InMemoryDbInlineAutoData(true)]
        [InMemoryDbInlineAutoData(false)]
        public static async Task UseDefaultImplementationPlan_UpdatesValue(
            bool value,
            int orderId,
            ContractFlags contractFlags,
            [Frozen] BuyingCatalogueDbContext dbContext,
            ContractsService service)
        {
            contractFlags.Order = null;
            contractFlags.OrderId = orderId;
            contractFlags.UseDefaultImplementationPlan = null;

            dbContext.ContractFlags.Add(contractFlags);

            await dbContext.SaveChangesAsync();

            await service.UseDefaultImplementationPlan(orderId, value);

            var actual = await dbContext.ContractFlags.FirstAsync(x => x.Id == contractFlags.Id);

            actual.UseDefaultImplementationPlan.Should().Be(value);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetContract_RemoveContract(
            int orderId,
            [Frozen] BuyingCatalogueDbContext dbContext)
        {
            var flags = new ContractFlags
            {
                OrderId = orderId,
                UseDefaultImplementationPlan = true,
                UseDefaultBilling = true,
                HasSpecificRequirements = true,
                UseDefaultDataProcessing = true,
            };
            dbContext.ContractFlags.Add(flags);

            await dbContext.SaveChangesAsync();
            ContractsService service = new ContractsService(dbContext);
            await service.RemoveContract(orderId);
            var output = await service.GetContract(orderId);

            output.UseDefaultImplementationPlan.Should().BeNull();
            output.UseDefaultBilling.Should().BeNull();
            output.HasSpecificRequirements.Should().BeNull();
            output.UseDefaultDataProcessing.Should().BeFalse();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetContract_RemoveBillingAndRequirements(
            int orderId,
            [Frozen] BuyingCatalogueDbContext dbContext)
        {
            var flags = new ContractFlags
            {
                OrderId = orderId,
                UseDefaultBilling = true,
                HasSpecificRequirements = true,
            };
            dbContext.ContractFlags.Add(flags);

            await dbContext.SaveChangesAsync();
            ContractsService service = new ContractsService(dbContext);
            await service.RemoveBillingAndRequirements(orderId);
            var output = await service.GetContract(orderId);

            output.UseDefaultBilling.Should().BeNull();
            output.HasSpecificRequirements.Should().BeNull();
        }
    }
}
