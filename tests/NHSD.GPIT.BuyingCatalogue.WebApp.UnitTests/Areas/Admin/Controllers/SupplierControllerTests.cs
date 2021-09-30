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
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Suppliers;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.SupplierModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class SupplierControllerTests
    {
        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(SuppliersController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_GetsAllSuppliers(
            IReadOnlyList<Supplier> suppliers,
            [Frozen] Mock<ISuppliersService> mockSuppliersService,
            SuppliersController controller)
        {
            mockSuppliersService.Setup(s => s.GetAllSuppliers()).ReturnsAsync(suppliers);

            await controller.Index();

            mockSuppliersService.Verify(o => o.GetAllSuppliers());
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Index_ReturnsViewWithExpectedViewModel(
            IReadOnlyList<Supplier> suppliers,
            [Frozen] Mock<ISuppliersService> mockSuppliersService,
            SuppliersController controller)
        {
            var expectedResult = new ManageSuppliersModel(suppliers);

            mockSuppliersService.Setup(o => o.GetAllSuppliers()).ReturnsAsync(suppliers);

            var actual = (await controller.Index()).As<ViewResult>();

            actual.Should().NotBeNull();
            actual.ViewName.Should().BeNullOrEmpty();
            actual.Model.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditSupplier_ReturnsViewWithExpectedViewModel(
            Supplier supplier,
            [Frozen] Mock<ISuppliersService> mockSuppliersService,
            SuppliersController controller)
        {
            var expectedResult = new EditSupplierModel(supplier)
            {
                BackLink = "testUrl",
                BackLinkText = "Go back",
            };

            mockSuppliersService.Setup(s => s.GetSupplier(It.IsAny<int>())).ReturnsAsync(supplier);

            var actual = (await controller.EditSupplier(It.IsAny<int>())).As<ViewResult>();

            actual.Should().NotBeNull();
            actual.ViewName.Should().BeNullOrEmpty();
            actual.Model.Should().BeEquivalentTo(expectedResult);

            mockSuppliersService.Verify(s => s.GetSupplier(It.IsAny<int>()));
        }
    }
}