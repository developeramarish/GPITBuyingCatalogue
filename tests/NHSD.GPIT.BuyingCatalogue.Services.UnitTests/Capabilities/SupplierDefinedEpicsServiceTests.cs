﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NHSD.GPIT.BuyingCatalogue.EntityFramework;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.SupplierDefinedEpics;
using NHSD.GPIT.BuyingCatalogue.Services.Capabilities;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.Services.UnitTests.Capabilities
{
    public static class SupplierDefinedEpicsServiceTests
    {
        private const string CapabilityName = "CapabilityName";
        private const string EpicName = "EpicName";
        private const string Junk = "Junk";

        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(SupplierDefinedEpicsService).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetSupplierDefinedEpics_ReturnsExpectedResult(
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            var expectedCount = context.Epics.Count(e => e.SupplierDefined);

            var supplierDefinedEpics = await service.GetSupplierDefinedEpics();

            supplierDefinedEpics.Count.Should().Be(expectedCount);
        }

        [Theory]
        [CommonInlineAutoData(null)]
        [CommonInlineAutoData("")]
        [CommonInlineAutoData(" ")]
        public static void GetSupplierDefinedEpicsBySearchTerm_SearchTermNull_ThrowsException(
            string searchTerm,
            SupplierDefinedEpicsService service)
        {
            FluentActions
                .Awaiting(() => service.GetSupplierDefinedEpicsBySearchTerm(searchTerm))
                .Should()
                .ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetSupplierDefinedEpicsBySearchTerm_ReturnsExpectedResult(
            string searchTerm,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            var epics = GetEpicsForSearchTerm(searchTerm);
            var noMatch = epics.First(x => x.Name == EpicName && x.Capabilities.Any(y => y.Name == CapabilityName));

            for (var i = 0; i < epics.Count; i++)
            {
                var capability = epics[i].Capabilities.First();

                capability.Id = i;
                capability.CapabilityRef = $"{i}";
                capability.Description = $"{i}";
                capability.Version = $"{i}";

                context.Capabilities.Add(capability);

                var epic = epics[i];

                epic.Id = $"S{i}";
                epic.SupplierDefined = true;

                context.Epics.Add(epic);
            }

            await context.SaveChangesAsync();

            var results = await service.GetSupplierDefinedEpicsBySearchTerm(searchTerm);

            foreach (var result in results)
            {
                epics.Should()
                    .Contain(
                        x => x.Name == result.Name && result.Capabilities.Any(y => x.Capabilities.Any(z => y.Name == z.Name)));
            }

            results.Should()
                .NotContain(
                    x => x.Name == noMatch.Name && noMatch.Capabilities.Any(y => x.Capabilities.Any(z => y.Name == z.Name)));
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task EpicExists_NullId_DuplicateContent_ReturnsTrue(
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            await context.SaveChangesAsync();

            var epicExists = await service.EpicExists(null, epic.Name, epic.Description, epic.IsActive);

            epicExists.Should().BeTrue();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task EpicExists_ValidId_DuplicateContent_ReturnsFalse(
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            await context.SaveChangesAsync();

            var epicExists = await service.EpicExists(
                epic.Id,
                epic.Name,
                epic.Description,
                epic.IsActive);

            epicExists.Should().BeFalse();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task EpicExists_NullId_ReturnsFalse(
            Epic epic,
            SupplierDefinedEpicsService service)
        {
            var epicExists = await service.EpicExists(null, epic.Name, epic.Description, epic.IsActive);

            epicExists.Should().BeFalse();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task EpicExists_ValidId_ReturnsFalse(
            Epic epic,
            SupplierDefinedEpicsService service)
        {
            var epicExists = await service.EpicExists(
                epic.Id,
                epic.Name,
                epic.Description,
                epic.IsActive);

            epicExists.Should().BeFalse();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static Task AddSupplierDefinedEpic_NullModel_ThrowsArgumentNullException(
            SupplierDefinedEpicsService service)
            => Assert.ThrowsAsync<ArgumentNullException>(() => service.AddSupplierDefinedEpic(null));

        [Theory]
        [InMemoryDbAutoData]
        public static async Task AddSupplierDefinedEpic_ValidModel_AddsEpic(
            Capability capability,
            AddEditSupplierDefinedEpic addEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            capability.Epics = new List<Epic>();

            context.Capabilities.Add(capability);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            addEpicModel.CapabilityId = capability.Id;

            await service.AddSupplierDefinedEpic(addEpicModel);

            var epic = await context.Epics.FirstAsync();

            epic.Id.Should().Be("S00001");
            epic.Name.Should().Be(addEpicModel.Name);
            epic.Description.Should().Be(addEpicModel.Description);
            epic.IsActive.Should().Be(addEpicModel.IsActive);
            epic.SupplierDefined.Should().BeTrue();
            epic.CompliancyLevel.Should().Be(CompliancyLevel.May);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task AddSupplierDefinedEpic_GeneratesIncrementedIds(
            Capability capability,
            AddEditSupplierDefinedEpic addFirstEpicModel,
            AddEditSupplierDefinedEpic addSecondEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            capability.Epics = new List<Epic>();

            context.Capabilities.Add(capability);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            addFirstEpicModel.CapabilityId = capability.Id;
            addSecondEpicModel.CapabilityId = capability.Id;

            await service.AddSupplierDefinedEpic(addFirstEpicModel);
            await service.AddSupplierDefinedEpic(addSecondEpicModel);

            var firstEpic = context.Epics.First();
            var secondEpic = context.Epics.Skip(1).First();

            firstEpic.Id.Should().Be("S00001");
            secondEpic.Id.Should().Be("S00002");
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task AddSupplierDefinedEpic_ExistingEpicInDb_IncrementsId(
            Capability capability,
            Epic epic,
            AddEditSupplierDefinedEpic addEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            capability.Epics = new List<Epic>();
            addEpicModel.CapabilityId = capability.Id;
            epic.Id = "S00012";
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            context.Capabilities.Add(capability);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await service.AddSupplierDefinedEpic(addEpicModel);

            var addedEpic = context.Epics.FirstOrDefault(e => string.Equals(e.Id, "S00013"));

            addedEpic.Should().NotBeNull();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task AddSupplierDefinedEpic_LegacyEpicInDb_IncrementsId(
            Capability capability,
            Epic epic,
            AddEditSupplierDefinedEpic addEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            capability.Epics = new List<Epic>();
            addEpicModel.CapabilityId = capability.Id;
            epic.Id = "S020X01E01";
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            context.Capabilities.Add(capability);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await service.AddSupplierDefinedEpic(addEpicModel);

            var addedEpic = context.Epics.FirstOrDefault(e => string.Equals(e.Id, "S00001"));

            addedEpic.Should().NotBeNull();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task AddSupplierDefinedEpic_LegacyAndNewEpicInDb_IncrementsId(
            Capability capability,
            Epic legacyEpic,
            Epic newEpic,
            AddEditSupplierDefinedEpic addEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            capability.Epics = new List<Epic>();
            addEpicModel.CapabilityId = capability.Id;

            legacyEpic.Id = "S020X01E01";
            legacyEpic.SupplierDefined = true;

            newEpic.Id = "S00010";
            newEpic.SupplierDefined = true;

            context.Epics.Add(legacyEpic);
            context.Epics.Add(newEpic);
            context.Capabilities.Add(capability);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await service.AddSupplierDefinedEpic(addEpicModel);

            var addedEpic = context.Epics.FirstOrDefault(e => string.Equals(e.Id, "S00011"));

            addedEpic.Should().NotBeNull();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetEpic_EpicIdNullOrEmpty_ThrowsArgumentException(
            SupplierDefinedEpicsService service)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetEpic(null));
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetEpic(string.Empty));
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetEpic_ValidEpicId_ReturnsEpic(
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            await context.SaveChangesAsync();

            var actualEpic = await service.GetEpic(epic.Id);

            actualEpic
                .Should()
                .BeEquivalentTo(epic);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetEpic_ValidEpicId_NotSupplierDefined_ReturnsNull(
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = false;

            context.Epics.Add(epic);
            await context.SaveChangesAsync();

            var actualEpic = await service.GetEpic(epic.Id);

            actualEpic.Should().BeNull();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetItemsReferencingEpic_EpicIdNullOrEmpty_ThrowsArgumentException(
            SupplierDefinedEpicsService service)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetItemsReferencingEpic(null));
            await Assert.ThrowsAsync<ArgumentException>(() => service.GetItemsReferencingEpic(string.Empty));
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetItemsReferencingEpic_ValidEpicId_ReturnsItems(
            Epic epic,
            Capability capability,
            List<CatalogueItem> catalogueItems,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.Capabilities = new List<Capability> { capability };
            epic.SupplierDefined = true;

            catalogueItems.ForEach(
                c => c.CatalogueItemEpics.Add(
                    new CatalogueItemEpic { CapabilityId = capability.Id, EpicId = epic.Id, StatusId = 1, }));

            context.Capabilities.Add(capability);
            context.Epics.Add(epic);
            context.CatalogueItems.AddRange(catalogueItems);

            await context.SaveChangesAsync();

            var relatedItems = await service.GetItemsReferencingEpic(epic.Id);

            relatedItems.Should().NotBeNull();
            relatedItems.Should().NotBeEmpty();
            relatedItems.Count.Should().Be(catalogueItems.Count);
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task GetItemsReferencingEpic_ValidEpicId_NotSupplierDefined_ReturnsEmpty(
            Epic epic,
            Capability capability,
            List<CatalogueItem> catalogueItems,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.Capabilities = new List<Capability> { capability };
            epic.SupplierDefined = false;

            catalogueItems.ForEach(
                c => c.CatalogueItemEpics.Add(
                    new CatalogueItemEpic { CapabilityId = capability.Id, EpicId = epic.Id, StatusId = 1, }));

            context.Capabilities.Add(capability);
            context.Epics.Add(epic);
            context.CatalogueItems.AddRange(catalogueItems);

            await context.SaveChangesAsync();

            var relatedItems = await service.GetItemsReferencingEpic(epic.Id);

            relatedItems.Should().NotBeNull();
            relatedItems.Should().BeEmpty();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static Task EditSupplierDefinedEpic_NullModel_ThrowsArgumentNullException(
            SupplierDefinedEpicsService service)
            => Assert.ThrowsAsync<ArgumentNullException>(() => service.EditSupplierDefinedEpic(null));

        [Theory]
        [InMemoryDbAutoData]
        public static Task EditSupplierDefinedEpic_InvalidEpicId_ThrowsKeyNotFoundException(
            AddEditSupplierDefinedEpic editEpicModel,
            SupplierDefinedEpicsService service)
        {
            editEpicModel.Id = "invalid-id";

            return Assert.ThrowsAsync<KeyNotFoundException>(() => service.EditSupplierDefinedEpic(editEpicModel));
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task EditSupplierDefinedEpic_ValidRequest_UpdatesEpic(
            Capability capability,
            Epic epic,
            AddEditSupplierDefinedEpic editEpicModel,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            var expectedEpic = new Epic
            {
                Id = epic.Id,
                Name = editEpicModel.Name,
                Description = editEpicModel.Description,
                IsActive = editEpicModel.IsActive,
            };

            capability.Epics = new List<Epic>();
            editEpicModel.CapabilityId = capability.Id;
            editEpicModel.Id = epic.Id;
            epic.SupplierDefined = true;

            context.Epics.Add(epic);
            context.Capabilities.Add(capability);

            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            await service.EditSupplierDefinedEpic(editEpicModel);

            var updatedEpic = await context.Epics.FindAsync(editEpicModel.Id);

            updatedEpic
                .Should()
                .BeEquivalentTo(
                    expectedEpic,
                    opt => opt.Excluding(m => m.SourceUrl)
                        .Excluding(m => m.LastUpdated)
                        .Excluding(m => m.LastUpdatedBy)
                        .Excluding(m => m.SupplierDefined)
                        .Excluding(m => m.CompliancyLevel)
                        .Excluding(m => m.LastUpdatedByUser)
                        .Excluding(m => m.Capabilities));
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task DeleteSupplierDefinedEpic_ValidId_DeletesEpic(
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = true;

            context.Epics.Add(epic);

            await service.DeleteSupplierDefinedEpic(epic.Id);

            context.Epics.Any(e => e.Id == epic.Id).Should().BeFalse();
        }

        [Theory]
        [InMemoryDbAutoData]
        public static async Task DeleteSupplierDefinedEpic_InvalidId_Returns(
            string invalidEpicId,
            Epic epic,
            [Frozen] BuyingCatalogueDbContext context,
            SupplierDefinedEpicsService service)
        {
            epic.SupplierDefined = true;

            context.Epics.Add(epic);

            var expectedCount = context.Epics.AsNoTracking().Count();

            await service.DeleteSupplierDefinedEpic(invalidEpicId);

            context.Epics.AsNoTracking().Count().Should().Be(expectedCount);
        }

        private static List<Epic> GetEpicsForSearchTerm(string searchTerm)
        {
            return new List<Epic>
            {
                new() { Name = EpicName, Capabilities = new List<Capability> { new() { Name = CapabilityName } }, },
                new()
                {
                    Name = $"{searchTerm}", Capabilities = new List<Capability> { new() { Name = CapabilityName } },
                },
                new()
                {
                    Name = $"{searchTerm}{Junk}",
                    Capabilities = new List<Capability> { new() { Name = CapabilityName } },
                },
                new()
                {
                    Name = $"{Junk}{searchTerm}",
                    Capabilities = new List<Capability> { new() { Name = CapabilityName } },
                },
                new()
                {
                    Name = $"{Junk}{searchTerm}{Junk}",
                    Capabilities = new List<Capability> { new() { Name = CapabilityName } },
                },
                new() { Name = EpicName, Capabilities = new List<Capability> { new() { Name = $"{searchTerm}" } }, },
                new()
                {
                    Name = EpicName, Capabilities = new List<Capability> { new() { Name = $"{searchTerm}{Junk}" } },
                },
                new()
                {
                    Name = EpicName, Capabilities = new List<Capability> { new() { Name = $"{Junk}{searchTerm}" } },
                },
                new()
                {
                    Name = EpicName,
                    Capabilities = new List<Capability> { new() { Name = $"{Junk}{searchTerm}{Junk}" } },
                },
            };
        }
    }
}
