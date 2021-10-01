﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.AssociatedServices;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Suppliers;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.AssociatedServices;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class AssociatedServicesControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(AssociatedServicesController).Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "AdminOnly");
            typeof(AssociatedServicesController).Should()
                .BeDecoratedWith<AreaAttribute>(a => a.RouteValue == "Admin");
            typeof(AssociatedServicesController).Should()
                .BeDecoratedWith<RouteAttribute>(r => r.Template == "admin/catalogue-solutions/manage/{solutionId}/associated-services");
        }

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
        public static async Task Get_AssociatedServices_ValidId_ReturnsViewWithExpectedModel(
            CatalogueItem catalogueItem,
            List<CatalogueItem> associatedServices,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            mockAssociatedServicesService.Setup(s => s.GetAssociatedServicesForSupplier(catalogueItem.Supplier.Id))
                .ReturnsAsync(associatedServices);

            var actual = await controller.AssociatedServices(catalogueItem.Id);

            actual.Should().BeOfType<ViewResult>();

            mockSolutionService.Verify(s => s.GetSolution(catalogueItem.Id));
            actual.As<ViewResult>().ViewName.Should().BeNull();
            actual.As<ViewResult>().Model.Should().BeEquivalentTo(new AssociatedServicesModel(catalogueItem, associatedServices));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AssociatedServices_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            AssociatedServicesController controller)
        {
            mockService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.AssociatedServices(catalogueItemId);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AssociatedServices_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            AssociatedServicesModel model,
            [Frozen] Mock<IAssociatedServicesService> mockService,
            AssociatedServicesController controller)
        {
            var actual = await controller.AssociatedServices(catalogueItemId, model);

            mockService.Verify(s => s.RelateAssociatedServicesToSolution(catalogueItemId, It.Is<IEnumerable<CatalogueItemId>>(
                l => l.SequenceEqual(model.SelectableAssociatedServices
                    .Where(a => a.Selected)
                    .Select(a => a.CatalogueItemId)))));

            actual.Should().BeOfType<RedirectToActionResult>();

            actual.As<RedirectToActionResult>().ActionName.Should().Be(nameof(CatalogueSolutionsController.ManageCatalogueSolution));
            actual.As<RedirectToActionResult>().ControllerName.Should().Be(typeof(CatalogueSolutionsController).ControllerName());
            actual.As<RedirectToActionResult>().RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddAssociatedService_ValidId_ReturnsViewWithExpectedModel(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = await controller.AddAssociatedService(catalogueItem.Id);

            mockSolutionService.Verify(s => s.GetSolution(catalogueItem.Id));

            actual.Should().BeOfType<ViewResult>();

            actual.As<ViewResult>().ViewName.Should().BeNull();
            actual.As<ViewResult>().Model.Should().BeEquivalentTo(new AddAssociatedServiceModel(catalogueItem));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddAssociatedService_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            AssociatedServicesController controller)
        {
            mockService.Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.AddAssociatedService(catalogueItemId);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedService_ValidIds_ReturnsViewWithExpectedModel(
            CatalogueItem solution,
            CatalogueItem associatedService,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(solution.Id))
                .ReturnsAsync(solution);

            mockAssociatedServicesService.Setup(s => s.GetAssociatedService(associatedService.Id))
                .ReturnsAsync(associatedService);

            var actual = await controller.EditAssociatedService(solution.Id, associatedService.Id);

            mockSolutionService.Verify(s => s.GetSolution(solution.Id));
            mockAssociatedServicesService.Verify(a => a.GetAssociatedService(associatedService.Id));

            actual.Should().BeOfType<ViewResult>();

            actual.As<ViewResult>().ViewName.Should().BeNull();
            actual.As<ViewResult>().Model.Should().BeEquivalentTo(new EditAssociatedServiceModel(solution, associatedService));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedService_InvalidSolutionId_ReturnsBadRequestResult(
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(solutionId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedService(solutionId, associatedServiceId);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {solutionId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedService_InvalidAssociatedServiceId_ReturnsBadRequestResult(
            CatalogueItem solution,
            CatalogueItemId associatedServiceId,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(solution.Id))
                .ReturnsAsync(solution);

            mockAssociatedServicesService.Setup(s => s.GetAssociatedService(associatedServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedService(solution.Id, associatedServiceId);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Associated Service found for Id: {associatedServiceId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedServiceDetails_ValidIds_ReturnsViewWithExpectedModel(
            CatalogueItem solution,
            CatalogueItem associatedService,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(solution.Id))
                .ReturnsAsync(solution);

            mockAssociatedServicesService.Setup(s => s.GetAssociatedService(associatedService.Id))
                .ReturnsAsync(associatedService);

            var actual = await controller.EditAssociatedServiceDetails(solution.Id, associatedService.Id);

            mockSolutionService.Verify(s => s.GetSolution(solution.Id));
            mockAssociatedServicesService.Verify(a => a.GetAssociatedService(associatedService.Id));

            actual.Should().BeOfType<ViewResult>();

            actual.As<ViewResult>().ViewName.Should().BeNull();
            actual.As<ViewResult>().Model.Should().BeEquivalentTo(new EditAssociatedServiceDetailsModel(solution, associatedService));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedServiceDetails_InvalidSolutionId_ReturnsBadRequestResult(
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            AssociatedServicesController controller)
        {
            mockSolutionService.Setup(s => s.GetSolution(solutionId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedServiceDetails(solutionId, associatedServiceId);

            actual.Should().BeOfType<BadRequestObjectResult>();
            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {solutionId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditAssociatedServicesDetails_InvalidAssociatedServiceId_ReturnsBadRequestResult(
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedService,
            AssociatedServicesController controller,
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId)
        {
            mockAssociatedService.Setup(s => s.GetAssociatedService(associatedServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedServiceDetails(solutionId, associatedServiceId);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Associated Service found for Id: {associatedServiceId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAssociatedServiceDetails_Valid_ReturnsViewWithExpectedRouteValues(
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller,
            CatalogueItem solution,
            CatalogueItem associatedService)
        {
            var model = new EditAssociatedServiceDetailsModel(solution, associatedService);

            mockAssociatedServicesService.Setup(s => s.EditDetails(
                associatedService.Id,
                new AssociatedServicesDetailsModel
                {
                    Name = model.Name,
                    Description = model.Description,
                    OrderGuidance = model.OrderGuidance,
                    UserId = It.IsAny<int>(),
                }));

            var actual = await controller.EditAssociatedServiceDetails(solution.Id, associatedService.Id, model);

            actual.Should().BeOfType<RedirectToActionResult>();

            actual.As<RedirectToActionResult>().ActionName.Should().Be(nameof(AssociatedServicesController.EditAssociatedService));
            actual.As<RedirectToActionResult>().RouteValues["solutionId"].Should().Be(solution.Id);
            actual.As<RedirectToActionResult>().RouteValues["associatedServiceId"].Should().Be(associatedService.Id);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAssociatedServiceDetails_InvalidSolutionId_ReturnsBadRequestResult(
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            AssociatedServicesController controller,
            EditAssociatedServiceDetailsModel model,
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId)
        {
            mockSolutionService.Setup(s => s.GetSolution(solutionId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedServiceDetails(solutionId, associatedServiceId, model);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Solution found for Id: {solutionId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAssociatedServiceDetails_InvalidAssociatedServiceId_ReturnsBadRequestResult(
            [Frozen] Mock<IAssociatedServicesService> mockService,
            AssociatedServicesController controller,
            EditAssociatedServiceDetailsModel model,
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId)
        {
            mockService.Setup(s => s.GetAssociatedService(associatedServiceId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = await controller.EditAssociatedServiceDetails(solutionId, associatedServiceId, model);

            actual.Should().BeOfType<BadRequestObjectResult>();

            actual.As<BadRequestObjectResult>().Value.Should().Be($"No Associated Service found for Id: {associatedServiceId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditAssociatedServiceDetails_MatchingName_ReturnsModelError(
            [Frozen] Mock<ISuppliersService> mockSupplierService,
            [Frozen] Mock<ISolutionsService> mockSolutionService,
            [Frozen] Mock<IAssociatedServicesService> mockAssociatedServicesService,
            AssociatedServicesController controller,
            CatalogueItem item,
            CatalogueItem solution,
            CatalogueItem associatedService,
            CatalogueItemId solutionId,
            CatalogueItemId associatedServiceId)
        {
            mockSolutionService.Setup(s => s.GetSolution(solutionId))
                    .ReturnsAsync(solution);

            mockAssociatedServicesService.Setup(s => s.GetAssociatedService(associatedServiceId))
                .ReturnsAsync(associatedService);

            var model = new EditAssociatedServiceDetailsModel(solution, associatedService);

            item.Id = solutionId;
            item.Name = model.Name;

            mockSupplierService.Setup(s => s.GetAllSolutionsForSupplier(associatedServiceId.SupplierId)).ReturnsAsync(new List<CatalogueItem> { item });

            var actual = await controller.EditAssociatedServiceDetails(solutionId, associatedServiceId, model);

            actual.Should().BeOfType<ViewResult>();

            actual.As<ViewResult>().Model.Should().NotBeNull();
            actual.As<ViewResult>().Model.Should().BeEquivalentTo(model);
        }
    }
}
