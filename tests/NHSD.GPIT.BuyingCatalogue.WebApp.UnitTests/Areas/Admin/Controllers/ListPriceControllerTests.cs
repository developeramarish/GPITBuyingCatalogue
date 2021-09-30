﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ListPriceModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class ListPriceControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(ListPriceController).Should().BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "AdminOnly");
            typeof(ListPriceController).Should().BeDecoratedWith<AreaAttribute>(a => a.RouteValue == "Admin");
            typeof(ListPriceController).Should().BeDecoratedWith<RouteAttribute>(r => r.Template == "admin/catalogue-solutions/manage/{solutionId}/list-prices");
        }

        [Fact]
        public static void Constructor_NullSolutionsService_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _ = new ListPriceController(null));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_ValidId_ReturnsCataloguePrice(
            CatalogueItemId catalogueItemId,
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.Index(catalogueItemId)).As<ViewResult>();

            mockSolutionsService.Verify(s => s.GetSolution(catalogueItemId));
            actual.ViewName.Should().BeNull();

            var manageListPricesModel = actual.Model.As<ManageListPricesModel>();
            manageListPricesModel.CataloguePrices.Should().BeEquivalentTo(catalogueItem.CataloguePrices);
            manageListPricesModel.CatalogueItemId.Should().Be(catalogueItem.Id);
            manageListPricesModel.CatalogueName.Should().Be(catalogueItem.Name);
            manageListPricesModel.BackLinkText.Should().Be("Go back");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await listPriceController.Index(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddListPrice_ValidId_ReturnsModel(
            CatalogueItemId catalogueItemId,
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.AddListPrice(catalogueItemId)).As<ViewResult>();

            mockSolutionsService.Verify(s => s.GetSolution(catalogueItemId));
            actual.ViewName.Should().NotBeNull();

            var model = actual.Model.As<EditListPriceModel>();
            model.SolutionId.Should().Be(catalogueItem.Id);
            model.SolutionName.Should().Be(catalogueItem.Name);
            model.BackLinkText.Should().Be("Go back");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AddListPrice_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await listPriceController.AddListPrice(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AddListPrice_ModelStateValid_RedirectsToManageListPrices(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            const decimal price = 3.21M;
            var solutionId = catalogueItem.Id;
            var editListPriceModel = new EditListPriceModel
            {
                SolutionName = catalogueItem.Name,
                Price = price,
                SelectedProvisioningType = ProvisioningType.Patient,
                Unit = "per patient",
            };

            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.AddListPrice(solutionId, editListPriceModel)).As<RedirectToActionResult>();

            mockSolutionsService.Verify(s => s.SaveSolutionListPrice(catalogueItem.Id, It.IsAny<SaveSolutionListPriceModel>()));

            actual.Should().NotBeNull();
            actual.ControllerName.Should().BeNull();
            actual.ActionName.Should().Be(nameof(ListPriceController.Index));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditListPrice_ValidId_ReturnsModel(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            var cataloguePriceId = catalogueItem
                .CataloguePrices
                .First()
                .CataloguePriceId;

            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.EditListPrice(catalogueItem.Id, cataloguePriceId)).As<ViewResult>();

            mockSolutionsService.Verify(s => s.GetSolution(catalogueItem.Id));
            actual.ViewName.Should().NotBeNull();

            var model = actual.Model.As<EditListPriceModel>();
            model.SolutionName.Should().Be(catalogueItem.Name);
            model.CataloguePriceId.Should().Be(cataloguePriceId);
            model.BackLinkText.Should().Be("Go back");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditListPrice_InvalidListPriceId_RedirectsToManageListPrices(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            const int cataloguePriceId = int.MaxValue;

            mockSolutionsService
               .Setup(s => s.GetSolution(catalogueItem.Id))
               .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.EditListPrice(catalogueItem.Id, cataloguePriceId)).As<RedirectToActionResult>();

            actual.Should().NotBeNull();
            actual.ControllerName.Should().BeNull();
            actual.ActionName.Should().Be(nameof(ListPriceController.Index));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditListPrice_ModelStateValid_RedirectsToManageListPrices(
            CatalogueItem catalogueItem,
            [Frozen] Mock<ISolutionsService> mockSolutionsService,
            ListPriceController listPriceController)
        {
            const decimal price = 3.21M;
            var solutionId = catalogueItem.Id;
            var cataloguePriceId = catalogueItem
                .CataloguePrices
                .First()
                .CataloguePriceId;

            var editListPriceModel = new EditListPriceModel
            {
                CataloguePriceId = cataloguePriceId,
                SolutionName = catalogueItem.Name,
                Price = price,
                SelectedProvisioningType = ProvisioningType.Patient,
                Unit = "per patient",
            };

            mockSolutionsService
                .Setup(s => s.GetSolution(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await listPriceController.EditListPrice(solutionId, cataloguePriceId, editListPriceModel)).As<RedirectToActionResult>();

            mockSolutionsService.Verify(s => s.UpdateSolutionListPrice(catalogueItem.Id, It.IsAny<SaveSolutionListPriceModel>()));

            actual.Should().NotBeNull();
            actual.ControllerName.Should().BeNull();
            actual.ActionName.Should().Be(nameof(ListPriceController.Index));
        }
    }
}