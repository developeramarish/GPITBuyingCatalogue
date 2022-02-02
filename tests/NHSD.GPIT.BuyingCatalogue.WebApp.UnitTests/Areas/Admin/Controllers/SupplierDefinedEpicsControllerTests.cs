﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Capabilities;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.SupplierDefinedEpics;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.SupplierDefinedEpics;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class SupplierDefinedEpicsControllerTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(SupplierDefinedEpicsController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Dashboard_ReturnsModelWithEpics(
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            SupplierDefinedEpicsController controller)
        {
            var epics = new List<Epic>
            {
                new Epic
                {
                    Capability = new Capability
                    {
                        Name = "Test Capability",
                    },
                    Name = "Test Epic",
                    Id = "S00001",
                },
            };

            var expectedModel = new SupplierDefinedEpicsDashboardModel(epics);

            supplierDefinedEpicsService.Setup(s => s.GetSupplierDefinedEpics())
                .ReturnsAsync(epics);

            var result = (await controller.Dashboard()).As<ViewResult>();

            result.Should().NotBeNull();

            var model = result.Model.As<SupplierDefinedEpicsDashboardModel>();
            model.Should().NotBeNull();
            model.Should().BeEquivalentTo(expectedModel);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddEpic_ReturnsModelWithCapabilities(
            List<Capability> capabilities,
            [Frozen] Mock<ICapabilitiesService> capabilitiesService,
            SupplierDefinedEpicsController controller)
        {
            var expectedCapabilitiesSelectList = new SelectList(
                capabilities
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                "Value",
                "Text");

            capabilitiesService.Setup(s => s.GetCapabilities())
                .ReturnsAsync(capabilities);

            var result = (await controller.AddEpic()).As<ViewResult>();

            result.Should().NotBeNull();

            var model = result.Model.As<SupplierDefinedEpicBaseModel>();

            model.Should().NotBeNull();

            model.Capabilities.Should().BeEquivalentTo(expectedCapabilitiesSelectList);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddEpic_InvalidModel_ReturnsView(
            SupplierDefinedEpicBaseModel model,
            SupplierDefinedEpicsController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var result = (await controller.AddEpic(model)).As<ViewResult>();

            result.Should().NotBeNull();
            result.Model.Should().BeEquivalentTo(model, opt => opt.Excluding(m => m.Capabilities));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddEpic_InvalidModel_RepopulatesCapabilities(
            List<Capability> capabilities,
            SupplierDefinedEpicBaseModel model,
            [Frozen] Mock<ICapabilitiesService> capabilitiesService,
            SupplierDefinedEpicsController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var expectedCapabilitiesSelectList = new SelectList(
                capabilities
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                "Value",
                "Text");

            capabilitiesService.Setup(s => s.GetCapabilities())
                .ReturnsAsync(capabilities);

            var result = (await controller.AddEpic(model)).As<ViewResult>();

            result.Should().NotBeNull();

            var viewModel = result.Model.As<SupplierDefinedEpicBaseModel>();

            viewModel.Should().NotBeNull();

            viewModel.Capabilities.Should().BeEquivalentTo(expectedCapabilitiesSelectList);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddEpic_ValidModel_AddsSupplierDefinedEpic(
            SupplierDefinedEpicBaseModel model,
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            SupplierDefinedEpicsController controller)
        {
            _ = await controller.AddEpic(model);

            supplierDefinedEpicsService.Verify(
                s => s.AddSupplierDefinedEpic(
                It.Is<AddEditSupplierDefinedEpic>(
                    m => m.CapabilityId == model.SelectedCapabilityId!.Value
                         && m.Name == model.Name
                         && m.Description == model.Description
                         && m.IsActive == model.IsActive!.Value)),
                Times.Once);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddEpic_ValidModel_RedirectsToDashboard(
            SupplierDefinedEpicBaseModel model,
            SupplierDefinedEpicsController controller)
        {
            var result = (await controller.AddEpic(model)).As<RedirectToActionResult>();

            result.Should().NotBeNull();
            result.ActionName.Should().Be(nameof(controller.Dashboard));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditEpic_EpicNotFound_ReturnsBadRequestObjectResult(
            string epicId,
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            SupplierDefinedEpicsController controller)
        {
            supplierDefinedEpicsService.Setup(s => s.GetEpic(epicId))
                .ReturnsAsync((Epic)null);

            var result = (await controller.EditEpic(epicId)).As<BadRequestObjectResult>();

            result.Should().NotBeNull();
            result.Value.Should().Be($"No Supplier defined Epic found for Id: {epicId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditEpic_Valid_ReturnsModel(
            Epic epic,
            List<Capability> capabilities,
            List<CatalogueItem> relatedItems,
            [Frozen] Mock<ICapabilitiesService> capabilitiesService,
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            SupplierDefinedEpicsController controller)
        {
            capabilitiesService.Setup(s => s.GetCapabilities())
                .ReturnsAsync(capabilities);

            supplierDefinedEpicsService.Setup(s => s.GetEpic(epic.Id))
                .ReturnsAsync(epic);

            supplierDefinedEpicsService.Setup(s => s.GetItemsReferencingEpic(epic.Id))
                .ReturnsAsync(relatedItems);

            var expectedModel = new EditSupplierDefinedEpicModel(epic, relatedItems)
                .WithSelectListCapabilities(capabilities);

            var result = (await controller.EditEpic(epic.Id)).As<ViewResult>();

            result.Should().NotBeNull();
            result.Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditEpic_InvalidModel_ReturnsView(
            EditSupplierDefinedEpicModel model,
            SupplierDefinedEpicsController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var result = (await controller.EditEpic(model.Id, model)).As<ViewResult>();

            result.Should().NotBeNull();
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditEpic_InvalidModel_RepopulatesCapabilities(
            List<Capability> capabilities,
            [Frozen] Mock<ICapabilitiesService> capabilitiesService,
            EditSupplierDefinedEpicModel model,
            SupplierDefinedEpicsController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var expectedCapabilitiesSelectList = new SelectList(
                capabilities
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                "Value",
                "Text");

            capabilitiesService.Setup(s => s.GetCapabilities())
                .ReturnsAsync(capabilities);

            var result = (await controller.EditEpic(model.Id, model)).As<ViewResult>();

            result.Should().NotBeNull();

            var viewModel = result.Model.As<EditSupplierDefinedEpicModel>();

            viewModel.Capabilities.Should().BeEquivalentTo(expectedCapabilitiesSelectList);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditEpic_InvalidModel_RepopulatesRelatedItems(
            List<CatalogueItem> relatedItems,
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            EditSupplierDefinedEpicModel model,
            SupplierDefinedEpicsController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            supplierDefinedEpicsService.Setup(s => s.GetItemsReferencingEpic(model.Id))
                .ReturnsAsync(relatedItems);

            var result = (await controller.EditEpic(model.Id, model)).As<ViewResult>();

            result.Should().NotBeNull();

            var viewModel = result.Model.As<EditSupplierDefinedEpicModel>();

            viewModel.RelatedItems.Should().BeEquivalentTo(relatedItems);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditEpic_ValidModel_EditsSupplierDefinedEpic(
            EditSupplierDefinedEpicModel model,
            [Frozen] Mock<ISupplierDefinedEpicsService> supplierDefinedEpicsService,
            SupplierDefinedEpicsController controller)
        {
            _ = await controller.EditEpic(model.Id, model);

            supplierDefinedEpicsService.Verify(
                s => s.EditSupplierDefinedEpic(
                It.Is<AddEditSupplierDefinedEpic>(
                    m => m.Id == model.Id
                         && m.CapabilityId == model.SelectedCapabilityId!.Value
                         && m.Name == model.Name
                         && m.Description == model.Description
                         && m.IsActive == model.IsActive!.Value)),
                Times.Once);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditEpic_ValidModel_RedirectsToDashboard(
            EditSupplierDefinedEpicModel model,
            SupplierDefinedEpicsController controller)
        {
            var result = (await controller.EditEpic(model.Id, model)).As<RedirectToActionResult>();

            result.Should().NotBeNull();
            result.ActionName.Should().Be(nameof(controller.Dashboard));
        }
    }
}