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
using Microsoft.AspNetCore.Routing;
using Moq;
using MoreLinq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Contracts;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Orders;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Routing;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.AutoFixtureCustomisations;
using NHSD.GPIT.BuyingCatalogue.UnitTest.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers.Contracts;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.Contracts.DeliveryDates;
using Xunit;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.UnitTests.Areas.Order.Controllers.Contracts
{
    public static class DeliveryDatesControllerTests
    {
        [Fact]
        public static void ClassIsCorrectlyDecorated()
        {
            typeof(DeliveryDatesController).Should().BeDecoratedWith<AuthorizeAttribute>();
            typeof(DeliveryDatesController).Should().BeDecoratedWith<AreaAttribute>(a => a.RouteValue == "Orders");
        }

        [Fact]
        public static void Constructors_VerifyGuardClauses()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var assertion = new GuardClauseAssertion(fixture);
            var constructors = typeof(DeliveryDatesController).GetConstructors();

            assertion.Verify(constructors);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_SelectDate_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.SelectDate(internalOrgId, callOffId);

            orderService.VerifyAll();

            var expected = new SelectDateModel(internalOrgId, callOffId, order.CommencementDate!.Value, order.DeliveryDate);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectDate_WithModelErrors_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            SelectDateModel model,
            DeliveryDatesController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            var result = await controller.SelectDate(internalOrgId, callOffId, model);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(model, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectDate_OrderDeliveryDateIsNull_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            SelectDateModel model,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IDeliveryDateService> deliveryDatesService,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();
            order.DeliveryDate = null;

            deliveryDatesService
                .Setup(x => x.SetDeliveryDate(internalOrgId, callOffId, model.Date.Value))
                .Verifiable();

            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.SelectDate(internalOrgId, callOffId, model);

            deliveryDatesService.VerifyAll();
            orderService.VerifyAll();

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.EditDates));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
                { "catalogueItemId", order.OrderItems.First().CatalogueItemId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectDate_NewDateSameAsOldDate_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            SelectDateModel model,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IDeliveryDateService> deliveryDatesService,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();
            order.DeliveryDate = model.Date;

            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.SelectDate(internalOrgId, callOffId, model);

            orderService.VerifyAll();
            deliveryDatesService.VerifyNoOtherCalls();

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.EditDates));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "InternalOrgId", internalOrgId },
                { "callOffId", callOffId },
                { "catalogueItemId", order.OrderItems.First().CatalogueItemId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_SelectDate_NewDateDifferentFromOldDate_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            SelectDateModel model,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();
            order.DeliveryDate = model.Date!.Value.AddDays(1);

            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.SelectDate(internalOrgId, callOffId, model);

            orderService.VerifyAll();

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.ConfirmChanges));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
                { "deliveryDate", model.Date.Value.ToString(DeliveryDatesController.DateFormat) },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_AmendDate_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            RoutingResult routingResult,
            [Frozen] Mock<IOrderService> orderService,
            [Frozen] Mock<IRoutingService> routingService,
            DeliveryDatesController controller)
        {
            var orderItem = order.OrderItems.First();
            var catalogueItemId = orderItem.CatalogueItemId;
            var date = orderItem.OrderItemRecipients.First().DeliveryDate;

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            RouteValues routeValues = null;

            routingService
                .Setup(x => x.GetRoute(RoutingPoint.AmendDeliveryDatesBackLink, order, It.IsAny<RouteValues>()))
                .Callback<RoutingPoint, EntityFramework.Ordering.Models.Order, RouteValues>((_, _, x) => routeValues = x)
                .Returns(routingResult);

            var result = await controller.AmendDate(internalOrgId, callOffId, catalogueItemId);

            orderService.VerifyAll();
            routingService.VerifyAll();

            routeValues.Should().BeEquivalentTo(new
            {
                InternalOrgId = internalOrgId,
                CallOffId = callOffId,
            });

            var expected = new AmendDateModel(internalOrgId, callOffId, catalogueItemId, order, date);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AmendDate_WithModelErrors_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            CatalogueItemId catalogueItemId,
            AmendDateModel model,
            DeliveryDatesController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            var result = await controller.AmendDate(internalOrgId, callOffId, catalogueItemId, model);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(model, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_AmendDate_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            CatalogueItemId catalogueItemId,
            AmendDateModel model,
            EntityFramework.Ordering.Models.Order order,
            RoutingResult routingResult,
            [Frozen] Mock<IDeliveryDateService> deliveryDatesService,
            [Frozen] Mock<IOrderService> orderService,
            [Frozen] Mock<IRoutingService> routingService,
            DeliveryDatesController controller)
        {
            model.Day = "1";
            model.Month = "1";
            model.Year = "2000";

            deliveryDatesService
                .Setup(x => x.SetDeliveryDate(internalOrgId, callOffId, catalogueItemId, model.Date.Value))
                .Verifiable();

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            RouteValues routeValues = null;

            routingService
                .Setup(x => x.GetRoute(RoutingPoint.AmendDeliveryDates, order, It.IsAny<RouteValues>()))
                .Callback<RoutingPoint, EntityFramework.Ordering.Models.Order, RouteValues>((_, _, x) => routeValues = x)
                .Returns(routingResult);

            var result = await controller.AmendDate(internalOrgId, callOffId, catalogueItemId, model);

            deliveryDatesService.VerifyAll();
            orderService.VerifyAll();
            routingService.VerifyAll();

            routeValues.Should().BeEquivalentTo(new
            {
                InternalOrgId = internalOrgId,
                CallOffId = callOffId,
                CatalogueItemId = catalogueItemId,
            });

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(routingResult.ActionName);
            actual.ControllerName.Should().Be(routingResult.ControllerName);
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_ConfirmChanges_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var newDate = order.DeliveryDate!.Value.Date.AddDays(1);

            var result = await controller.ConfirmChanges(
                internalOrgId,
                callOffId,
                newDate.ToString(DeliveryDatesController.DateFormat));

            orderService.VerifyAll();

            var expected = new ConfirmChangesModel(internalOrgId, callOffId, order.DeliveryDate.Value, newDate);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_ConfirmChanges_WithModelErrors_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            ConfirmChangesModel model,
            DeliveryDatesController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            var result = await controller.ConfirmChanges(internalOrgId, callOffId, model);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(model, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_ConfirmChanges_NoSelected_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            ConfirmChangesModel model,
            DeliveryDatesController controller)
        {
            model.ConfirmChanges = false;

            var result = await controller.ConfirmChanges(internalOrgId, callOffId, model);
            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.Review));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_ConfirmChanges_YesSelected_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            ConfirmChangesModel model,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IDeliveryDateService> deliveryDatesService,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();
            model.ConfirmChanges = true;

            deliveryDatesService
                .Setup(x => x.SetDeliveryDate(internalOrgId, callOffId, model.NewDeliveryDate))
                .Verifiable();

            orderService
                .Setup(x => x.GetOrderThin(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.ConfirmChanges(internalOrgId, callOffId, model);

            deliveryDatesService.VerifyAll();
            orderService.VerifyAll();

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.EditDates));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", model.InternalOrgId },
                { "callOffId", model.CallOffId },
                { "catalogueItemId", order.OrderItems.First().CatalogueItemId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_EditDates_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();
            order.OrderItems.ForEach(x => x.OrderItemRecipients.ForEach(r => r.Recipient = new ServiceRecipient { Name = "Recipient Name" }));

            var catalogueItemId = order.OrderItems.First().CatalogueItemId;

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.EditDates(internalOrgId, callOffId, catalogueItemId);

            orderService.VerifyAll();

            var expected = new EditDatesModel(order, catalogueItemId);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditDates_WithModelErrors_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            CatalogueItemId catalogueItemId,
            EditDatesModel model,
            DeliveryDatesController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            var result = await controller.EditDates(internalOrgId, callOffId, catalogueItemId, model);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(model, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_EditDates_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            EditDatesModel model,
            [Frozen] Mock<IDeliveryDateService> deliveryDatesService,
            [Frozen] Mock<IOrderService> orderService,
            [Frozen] Mock<IRoutingService> routingService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();

            var catalogueItemId = order.OrderItems.First().CatalogueItemId;

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var deliveryDates = new List<RecipientDeliveryDateDto>();
            var routeValues = new RouteValues();

            deliveryDatesService
                .Setup(x => x.SetDeliveryDates(order.Id, catalogueItemId, It.IsAny<List<RecipientDeliveryDateDto>>()))
                .Callback<int, CatalogueItemId, List<RecipientDeliveryDateDto>>((_, _, x) => deliveryDates = x);

            routingService
                .Setup(x => x.GetRoute(RoutingPoint.EditDeliveryDates, order, It.IsAny<RouteValues>()))
                .Callback<RoutingPoint, EntityFramework.Ordering.Models.Order, RouteValues>((_, _, x) => routeValues = x)
                .Returns(new RoutingResult
                {
                    ActionName = "action",
                    ControllerName = "controller",
                    RouteValues = new { internalOrgId, callOffId },
                });

            var result = await controller.EditDates(internalOrgId, callOffId, catalogueItemId, model);

            deliveryDatesService.VerifyAll();
            orderService.VerifyAll();
            routingService.VerifyAll();

            deliveryDates.Count.Should().Be(model.Recipients.Length);

            foreach (var deliveryDate in deliveryDates)
            {
                var recipient = model.Recipients.First(x => x.OdsCode == deliveryDate.OdsCode);
                recipient.Date.Should().Be(deliveryDate.DeliveryDate);
            }

            routeValues.InternalOrgId.Should().Be(internalOrgId);
            routeValues.CallOffId.Should().Be(callOffId);
            routeValues.CatalogueItemId.Should().Be(catalogueItemId);
            routeValues.Source.Should().Be(model.Source);

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be("action");
            actual.ControllerName.Should().Be("controller");
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_MatchDates_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            order.SetupCatalogueSolution();

            var catalogueItem = order.OrderItems.ElementAt(1).CatalogueItem;

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.MatchDates(internalOrgId, callOffId, catalogueItem.Id);

            orderService.VerifyAll();

            var expected = new MatchDatesModel(internalOrgId, callOffId, catalogueItem);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_MatchDates_WithModelErrors_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            CatalogueItemId catalogueItemId,
            MatchDatesModel model,
            DeliveryDatesController controller)
        {
            controller.ModelState.AddModelError("key", "errorMessage");

            var result = await controller.MatchDates(internalOrgId, callOffId, catalogueItemId, model);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(model, x => x.Excluding(m => m.BackLink));
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_MatchDates_MatchDatesIsTrue_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            MatchDatesModel model,
            [Frozen] Mock<IOrderService> orderService,
            [Frozen] Mock<IDeliveryDateService> deliveryDateService,
            DeliveryDatesController controller)
        {
            model.MatchDates = true;
            order.SetupCatalogueSolution();

            var solutionId = order.OrderItems.ElementAt(0).CatalogueItemId;
            var catalogueItemId = order.OrderItems.ElementAt(1).CatalogueItemId;

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            deliveryDateService
                .Setup(x => x.MatchDeliveryDates(order.Id, solutionId, catalogueItemId))
                .Verifiable();

            var result = await controller.MatchDates(internalOrgId, callOffId, catalogueItemId, model);

            orderService.VerifyAll();
            deliveryDateService.VerifyAll();

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.EditDates));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
                { "catalogueItemId", catalogueItemId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Post_MatchDates_MatchDatesIsFalse_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            MatchDatesModel model,
            [Frozen] Mock<IOrderService> orderService,
            [Frozen] Mock<IDeliveryDateService> deliveryDateService,
            DeliveryDatesController controller)
        {
            model.MatchDates = false;
            order.SetupCatalogueSolution();

            var catalogueItemId = order.OrderItems.ElementAt(1).CatalogueItemId;
            var orderItem = order.OrderItem(catalogueItemId);

            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var recipientDates = new List<RecipientDeliveryDateDto>();

            deliveryDateService
                .Setup(x => x.SetDeliveryDates(order.Id, catalogueItemId, It.IsAny<List<RecipientDeliveryDateDto>>()))
                .Callback<int, CatalogueItemId, List<RecipientDeliveryDateDto>>((_, _, x) => recipientDates = x);

            var result = await controller.MatchDates(internalOrgId, callOffId, catalogueItemId, model);

            orderService.VerifyAll();
            deliveryDateService.VerifyAll();

            recipientDates.Count.Should().Be(orderItem.OrderItemRecipients.Count);
            recipientDates.Select(x => x.OdsCode).Should().BeEquivalentTo(orderItem.OrderItemRecipients.Select(x => x.OdsCode));
            recipientDates.ForEach(x => x.DeliveryDate.Should().Be(order.DeliveryDate));

            var actual = result.Should().BeOfType<RedirectToActionResult>().Subject;

            actual.ActionName.Should().Be(nameof(DeliveryDatesController.EditDates));
            actual.ControllerName.Should().Be(typeof(DeliveryDatesController).ControllerName());
            actual.RouteValues.Should().BeEquivalentTo(new RouteValueDictionary
            {
                { "internalOrgId", internalOrgId },
                { "callOffId", callOffId },
                { "catalogueItemId", catalogueItemId },
            });
        }

        [Theory]
        [CommonAutoData]
        public static async Task Get_Review_ReturnsExpectedResult(
            string internalOrgId,
            CallOffId callOffId,
            EntityFramework.Ordering.Models.Order order,
            [Frozen] Mock<IOrderService> orderService,
            DeliveryDatesController controller)
        {
            orderService
                .Setup(x => x.GetOrderWithOrderItems(callOffId, internalOrgId))
                .ReturnsAsync(new OrderWrapper(order));

            var result = await controller.Review(internalOrgId, callOffId);

            orderService.VerifyAll();

            var expected = new ReviewModel(order);
            var actual = result.Should().BeOfType<ViewResult>().Subject;

            actual.Model.Should().BeEquivalentTo(expected, x => x.Excluding(m => m.BackLink));
        }
    }
}
