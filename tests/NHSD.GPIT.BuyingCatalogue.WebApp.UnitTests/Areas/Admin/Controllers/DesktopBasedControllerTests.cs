﻿using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Idioms;
using AutoFixture.Xunit2;
using EnumsNET;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.ApplicationTypeModels.DesktopBasedModels;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Admin.Controllers
{
    public static class DesktopBasedControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(DesktopBasedController).Should()
                .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "AdminOnly");
            typeof(DesktopBasedController).Should()
                .BeDecoratedWith<AreaAttribute>(a => a.RouteValue == "Admin");
            typeof(DesktopBasedController).Should()
                .BeDecoratedWith<RouteAttribute>(r => r.Template == "admin/catalogue-solutions");
        }

        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(DesktopBasedController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Desktop_ValidId_ReturnsViewWithExpectedModel(
         Solution solution,
         List<Integration> integrations,
         [Frozen] Mock<ISolutionsService> mockService,
         DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.Desktop(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new DesktopBasedModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Desktop_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.Desktop(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_OperatingSystems_ValidId_ReturnsViewWithExpectedModel(
           Solution solution,
           List<Integration> integrations,
           [Frozen] Mock<ISolutionsService> mockService,
           DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.OperatingSystems(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new OperatingSystemsModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_OperatingSystems_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.OperatingSystems(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_OperatingSystems_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            OperatingSystemsModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.OperatingSystems(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));
            applicationTypeDetail.NativeDesktopOperatingSystemsDescription = model.Description;

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Connectivity_ValidId_ReturnsViewWithExpectedModel(
           Solution solution,
           List<Integration> integrations,
           [Frozen] Mock<ISolutionsService> mockService,
           DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.Connectivity(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new ConnectivityModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Connectivity_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.Connectivity(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_Connectivity_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            ConnectivityModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.Connectivity(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.NativeDesktopMinimumConnectionSpeed = model.SelectedConnectionSpeed;

            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_MemoryAndStorage_ValidId_ReturnsViewWithExpectedModel(
           Solution solution,
           List<Integration> integrations,
           [Frozen] Mock<ISolutionsService> mockService,
           DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.MemoryAndStorage(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new MemoryAndStorageModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_MemoryAndStorage_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.MemoryAndStorage(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_MemoryAndStorage_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            MemoryAndStorageModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.MemoryAndStorage(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.NativeDesktopMemoryAndStorage.MinimumMemoryRequirement = model.SelectedMemorySize;
            applicationTypeDetail.NativeDesktopMemoryAndStorage.StorageRequirementsDescription = model.StorageSpace;
            applicationTypeDetail.NativeDesktopMemoryAndStorage.MinimumCpu = model.ProcessingPower;
            applicationTypeDetail.NativeDesktopMemoryAndStorage.RecommendedResolution = model.SelectedResolution;
            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_ThirdPartyComponents_ValidId_ReturnsViewWithExpectedModel(
            Solution solution,
            List<Integration> integrations,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.ThirdPartyComponents(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new ThirdPartyComponentsModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_ThirdPartyComponents_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.ThirdPartyComponents(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_ThirdPartyComponents_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            ThirdPartyComponentsModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.ThirdPartyComponents(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.NativeDesktopThirdParty.ThirdPartyComponents = model.ThirdPartyComponents;
            applicationTypeDetail.NativeDesktopThirdParty.DeviceCapabilities = model.DeviceCapabilities;
            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_HardwareRequirements_ValidId_ReturnsViewWithExpectedModel(
             Solution solution,
             List<Integration> integrations,
             [Frozen] Mock<ISolutionsService> mockService,
             DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.HardwareRequirements(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new HardwareRequirementsModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_HardwareRequirements_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.HardwareRequirements(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_HardwareRequirements_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            HardwareRequirementsModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.HardwareRequirements(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.NativeDesktopHardwareRequirements = model.Description;
            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AdditionalInformation_ValidId_ReturnsViewWithExpectedModel(
            Solution solution,
            List<Integration> integrations,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            var catalogueItem = solution.CatalogueItem;
            solution.Integrations = JsonSerializer.Serialize(integrations);

            mockService.Setup(s => s.GetSolutionThin(catalogueItem.Id))
                .ReturnsAsync(catalogueItem);

            var actual = (await controller.AdditionalInformation(catalogueItem.Id)).As<ViewResult>();

            mockService.Verify(s => s.GetSolutionThin(catalogueItem.Id));
            actual.ViewName.Should().BeNull();
            actual.Model.Should().BeEquivalentTo(new AdditionalInformationModel(catalogueItem), opt => opt.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AdditionalInformation_InvalidId_ReturnsBadRequestResult(
            CatalogueItemId catalogueItemId,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetSolutionThin(catalogueItemId))
                .ReturnsAsync(default(CatalogueItem));

            var actual = (await controller.AdditionalInformation(catalogueItemId)).As<BadRequestObjectResult>();

            actual.Value.Should().Be($"No Solution found for Id: {catalogueItemId}");
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AdditionalInformation_Saves_And_RedirectsToDesktop(
            CatalogueItemId catalogueItemId,
            AdditionalInformationModel model,
            ApplicationTypeDetail applicationTypeDetail,
            [Frozen] Mock<ISolutionsService> mockService,
            DesktopBasedController controller)
        {
            mockService.Setup(s => s.GetApplicationType(catalogueItemId)).ReturnsAsync(applicationTypeDetail);
            applicationTypeDetail.ApplicationTypes.Clear();

            var actual = (await controller.AdditionalInformation(catalogueItemId, model)).As<RedirectToActionResult>();

            applicationTypeDetail.NativeDesktopAdditionalInformation = model.AdditionalInformation;
            applicationTypeDetail.ApplicationTypes.Add(ApplicationType.Desktop.AsString(EnumFormat.EnumMemberValue));

            mockService.Verify(s => s.SaveApplicationType(catalogueItemId, applicationTypeDetail));
            actual.ActionName.Should().Be(nameof(DesktopBasedController.Desktop));
            actual.ControllerName.Should().BeNull();
            actual.RouteValues["solutionId"].Should().Be(catalogueItemId);
        }
    }
}
