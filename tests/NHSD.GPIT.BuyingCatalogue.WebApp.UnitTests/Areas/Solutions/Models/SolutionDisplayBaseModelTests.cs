﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.Test.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Solutions.Models
{
    public static class SolutionDisplayBaseModelTests
    {
        private static readonly IList<SectionModel> SectionModels = new List<SectionModel>
        {
            new()
            {
                Action = nameof(SolutionDetailsController.Description),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Description",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Features),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Features",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Capabilities),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Capabilities",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.ListPrice),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "List price",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.AdditionalServices),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Additional Services",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Description),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Associated Services",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Interoperability),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Interoperability",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Implementation),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Implementation",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.ClientApplicationTypes),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Client application type",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.HostingType),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Hosting type",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Description),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Service Level Agreement",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Description),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Development plans",
            },
            new()
            {
                Action = nameof(SolutionDetailsController.Description),
                Controller = typeof(SolutionDetailsController).ControllerName(),
                Name = "Supplier details",
            },
        };

        [Theory]
        [InlineData(typeof(ClientApplicationTypesModel))]
        [InlineData(typeof(ImplementationTimescalesModel))]
        [InlineData(typeof(SolutionDescriptionModel))]
        [InlineData(typeof(SolutionFeaturesModel))]
        public static void ChildClasses_InheritFrom_SolutionDisplayBaseModel(Type childType)
        {
            childType
            .Should()
            .BeAssignableTo<SolutionDisplayBaseModel>();
        }

        [Theory]
        [CommonAutoData]
        public static void GetSections_ValidSectionProperty_ReturnsSectionsWithSelected(CatalogueItemId solutionId)
        {
            var model = new TestSolutionDisplayBaseModel { SolutionId = solutionId, };
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 != 0) continue;
                model.SetShowTrue(i);
                SectionModels[i].Show = true;
            }

            var expected = new List<SectionModel>(SectionModels.Where(s => s.Show));
            expected.ForEach(s => s.Id = solutionId.ToString());
            expected.Single(s => s.Name.EqualsIgnoreCase(model.Section)).Selected = true;

            var actual = model.GetSections();

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("Description", false)]
        [InlineData("description", false)]
        [InlineData("DESCRIPTION", false)]
        [InlineData("Implementation", true)]
        [InlineData("Hosting", true)]
        public static void NotFirstSection_Returns_ExpectedResponse(string section, bool expected)
        {
            var model = new Mock<SolutionDisplayBaseModel> { CallBase = true };
            model.SetupGet(m => m.Section)
                .Returns(section);

            var actual = model.Object.NotFirstSection();

            model.VerifyGet(m => m.Section);
            actual.Should().Be(expected);
        }
    }
}
