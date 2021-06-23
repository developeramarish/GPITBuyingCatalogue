﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue;
using NHSD.GPIT.BuyingCatalogue.Framework.Logging;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models;
using NUnit.Framework;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    internal static class HomeControllerTests
    {
        [Test]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(HomeController).Should().BeDecoratedWith<AuthorizeAttribute>(x => x.Policy == "AdminOnly");
            typeof(HomeController).Should().BeDecoratedWith<AreaAttribute>(x => x.RouteValue == "Admin");
            typeof(HomeController).Should().BeDecoratedWith<RouteAttribute>(x => x.Template == "admin");
        }

        [Test]
        public static void Constructor_NullLogging_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                    () =>
                        _ = new HomeController(null, Mock.Of<IOrganisationsService>(), Mock.Of<IMapper>()))
                .ParamName.Should()
                .Be("logger");
        }
        
        [Test]
        public static void Constructor_NullOrganisationService_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                    () =>
                        _ = new HomeController(Mock.Of<ILogWrapper<HomeController>>(), null, Mock.Of<IMapper>()))
                .ParamName.Should()
                .Be("organisationsService");
        }

        [Test]
        public static void Constructor_NullMapper_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                    () =>
                        _ = new HomeController(
                            Mock.Of<ILogWrapper<HomeController>>(),
                            Mock.Of<IOrganisationsService>(),
                            null))
                .ParamName.Should()
                .Be("mapper");
        }
        
        [Test]
        public static void Get_BuyerOrganisations_RouteAttribute_ExpectedTemplate()
        {
            typeof(HomeController)
                .GetMethod(nameof(HomeController.BuyerOrganisations))
                .GetCustomAttribute<RouteAttribute>()
                .Template.Should()
                .Be("buyer-organisations");
        }

        [Test]
        public static async Task Get_BuyerOrganisations_GetsAllOrganisations()
        {
            var mockOrganisationService = new Mock<IOrganisationsService>();
            var controller = new HomeController(
                Mock.Of<ILogWrapper<HomeController>>(),
                mockOrganisationService.Object,
                Mock.Of<IMapper>());

            await controller.BuyerOrganisations();
            
            mockOrganisationService.Verify(o => o.GetAllOrganisations());
        }

        [Test]
        public static async Task Get_BuyerOrganisations_MapsOrganisationsToModels()
        {
            var mockOrganisationService = new Mock<IOrganisationsService>();
            var mockOrganisations = new Mock<IList<Organisation>>().Object;
            mockOrganisationService.Setup(o => o.GetAllOrganisations())
                .ReturnsAsync(mockOrganisations);
            var mockMapper = new Mock<IMapper>();
            var controller = new HomeController(
                Mock.Of<ILogWrapper<HomeController>>(),
                mockOrganisationService.Object,
                mockMapper.Object);

            await controller.BuyerOrganisations();

            mockMapper.Verify(m => m.Map<IList<Organisation>, IList<OrganisationModel>>(mockOrganisations));
        }

        [Test]
        public static async Task Get_BuyerOrganisations_ReturnsViewWithExpectedViewModel()
        {
            var mockOrganisationService = new Mock<IOrganisationsService>();
            var mockOrganisations = new Mock<IList<Organisation>>().Object;
            mockOrganisationService.Setup(o => o.GetAllOrganisations())
                .ReturnsAsync(mockOrganisations);
            var mockMapper = new Mock<IMapper>();
            var mockOrganisationModels = new Mock<IList<OrganisationModel>>().Object;
            mockMapper.Setup(m => m.Map<IList<Organisation>, IList<OrganisationModel>>(mockOrganisations))
                .Returns(mockOrganisationModels);
            var controller = new HomeController(
                Mock.Of<ILogWrapper<HomeController>>(),
                mockOrganisationService.Object,
                mockMapper.Object);

            var actual = (await controller.BuyerOrganisations()).As<ViewResult>();

            actual.Should().NotBeNull();
            actual.ViewName.Should().BeNullOrEmpty();
            Assert.AreSame(mockOrganisationModels, actual.Model);
        }

        [Test]
        public static void Get_Index_ReturnsDefaultView()
        {
            var controller = new HomeController(
                Mock.Of<ILogWrapper<HomeController>>(),
                Mock.Of<IOrganisationsService>(),
                Mock.Of<IMapper>());

            var result = controller.Index().As<ViewResult>();

            result.Should().NotBeNull();
            result.ViewName.Should().BeNull();
        }
    }
}