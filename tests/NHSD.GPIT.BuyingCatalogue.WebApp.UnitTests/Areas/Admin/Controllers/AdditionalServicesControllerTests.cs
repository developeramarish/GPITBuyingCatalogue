﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.AdditionalServices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.AdditionalServices;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class AdditionalServicesControllerTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(AssociatedServicesController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_WithInvalidSolutionId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.Index(catalogueItemId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_WithValidSolutionId_ReturnsModel(
            CatalogueItem catalogueItem,
            List<CatalogueItem> additionalServices,
            [Frozen] Mock<ISolutionsService> solutionsService,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            var expectedResult = new AdditionalServicesModel(catalogueItem, additionalServices);

            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            additionalServicesService.Setup(s => s.GetAdditionalServicesBySolutionId(catalogueItem.Id))
                .ReturnsAsync(additionalServices);

            var result = await controller.Index(catalogueItem.Id);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalService_InvalidSolutionId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalService(catalogueItemId, additionalServiceId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalService_InvalidAdditionalServiceId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItemId, additionalServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalService(catalogueItemId, additionalServiceId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Additional Service with Id {additionalServiceId} found for Solution {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalService_WithValidIds_ReturnsModel(
            CatalogueItem catalogueItem,
            CatalogueItem additionalService,
            [Frozen] Mock<ISolutionsService> solutionsService,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            var expectedResult = new EditAdditionalServiceModel(catalogueItem, additionalService);

            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItem.Id, additionalService.Id))
                .ReturnsAsync(additionalService);

            var result = await controller.EditAdditionalService(catalogueItem.Id, additionalService.Id);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(expectedResult, opt => opt.Excluding(model => model.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddAdditionalService_InvalidId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            AdditionalServicesController controller)
        {
            mockSolutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.AddAdditionalService(catalogueItemId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddAdditionalService_ValidId_ReturnsModel(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            var expectedModel = new EditAdditionalServiceDetailsModel(catalogueItem);

            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var result = await controller.AddAdditionalService(catalogueItem.Id);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(model => model.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddAdditionalService_InvalidModel_ReturnsViewWithModel(
            CatalogueItemId catalogueItemId,
            EditAdditionalServiceDetailsModel model,
            AdditionalServicesController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var result = await controller.AddAdditionalService(catalogueItemId, model);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(model, opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddAdditionalService_InvalidId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            EditAdditionalServiceDetailsModel model,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.AddAdditionalService(catalogueItemId, model);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddAdditionalService_ValidId_RedirectsToEditAdditionalService(
            CatalogueItem catalogueItem,
            EditAdditionalServiceDetailsModel model,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var result = await controller.AddAdditionalService(catalogueItem.Id, model);

            result.As<RedirectToActionResult>().Should().NotBeNull();
            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(AdditionalServicesController.EditAdditionalService));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalServiceDetails_InvalidCatalogueItemId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalServiceDetails(catalogueItemId, additionalServiceId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalServiceDetails_InvalidAdditionalServiceId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItemId, additionalServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalServiceDetails(catalogueItemId, additionalServiceId);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Additional Service with Id {additionalServiceId} found for Solution {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAdditionalServiceDetails_ValidIds_ReturnsModel(
            CatalogueItem catalogueItem,
            CatalogueItem additionalService,
            [Frozen] Mock<ISolutionsService> solutionsService,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            var expectedModel = new EditAdditionalServiceDetailsModel(catalogueItem, additionalService);

            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItem.Id, additionalService.Id))
                .ReturnsAsync(additionalService);

            var result = await controller.EditAdditionalServiceDetails(catalogueItem.Id, additionalService.Id);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(expectedModel, opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAdditionalServiceDetails_InvalidModel_ReturnsViewWithModel(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            EditAdditionalServiceDetailsModel model,
            AdditionalServicesController controller)
        {
            controller.ModelState.AddModelError("some-key", "some-error");

            var result = await controller.EditAdditionalServiceDetails(catalogueItemId, additionalServiceId, model);

            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().Model.Should().BeEquivalentTo(model, opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAdditionalServiceDetails_InvalidCatalogueItemId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            EditAdditionalServiceDetailsModel model,
            [Frozen] Mock<ISolutionsService> solutionsService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalServiceDetails(catalogueItemId, additionalServiceId, model);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAdditionalServiceDetails_InvalidAdditionalServiceId_ReturnsBadRequestObjectResult(
            CatalogueItemId catalogueItemId,
            CatalogueItemId additionalServiceId,
            EditAdditionalServiceDetailsModel model,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItemId, additionalServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var result = await controller.EditAdditionalServiceDetails(catalogueItemId, additionalServiceId, model);

            result.As<BadRequestObjectResult>().Should().NotBeNull();
            result.As<BadRequestObjectResult>().Value.Should().Be($"No Additional Service with Id {additionalServiceId} found for Solution {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAdditionalServiceDetails_ValidModel_RedirectsToEditAdditionalService(
            CatalogueItem catalogueItem,
            CatalogueItem additionalService,
            EditAdditionalServiceDetailsModel model,
            [Frozen] Mock<ISolutionsService> solutionsService,
            [Frozen] Mock<IAdditionalServicesService> additionalServicesService,
            AdditionalServicesController controller)
        {
            solutionsService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            additionalServicesService.Setup(s => s.GetAdditionalService(catalogueItem.Id, additionalService.Id))
                .ReturnsAsync(additionalService);

            var result = await controller.EditAdditionalServiceDetails(catalogueItem.Id, additionalService.Id, model);

            result.As<RedirectToActionResult>().Should().NotBeNull();
            result.As<RedirectToActionResult>().ActionName.Should().Be(nameof(AdditionalServicesController.EditAdditionalService));
        }
    }
}
