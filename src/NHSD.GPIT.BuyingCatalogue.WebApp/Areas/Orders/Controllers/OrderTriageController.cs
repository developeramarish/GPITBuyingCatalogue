﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.OrderTriage;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.Shared;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers
{
    [Authorize("Buyer")]
    [Area("Orders")]
    [Route("order/organisation/{internalOrgId}/order/triage")]
    public class OrderTriageController : Controller
    {
        private static readonly Dictionary<OrderTriageValue, (string Title, string Advice, string ValidationError)> TriageSelectionContent = new()
        {
            [OrderTriageValue.Under40K] = ("Have you identified what you want to order?",
                   "As your order is under £40k, you can execute a Direct Award. Any Catalogue Solution or service on the Buying Catalogue can be procured without carrying out a competition.",
                   "Select yes if you’ve identified what you want to order"),
            [OrderTriageValue.Between40KTo250K] = ("Have you carried out a competition using the Buying Catalogue?",
                   "As your order is between £40k and £250k, you should have executed an On-Catalogue Competition to identify the Catalogue Solution that best meets your needs.",
                   "Select yes if you’ve carried out a competition on the Buying Catalogue"),
            [OrderTriageValue.Over250K] = ("Have you sent out Invitations to Tender to suppliers?",
                   "As your order is over £250k, you should have executed an Off-Catalogue Competition to identify the Catalogue Solution that best meets your needs.",
                   "Select yes if you’ve carried out an Off-Catalogue Competition with suppliers"),
        };

        private readonly IOrganisationsService organisationsService;

        public OrderTriageController(IOrganisationsService organisationsService)
        {
            this.organisationsService = organisationsService ?? throw new ArgumentNullException(nameof(organisationsService));
        }

        [HttpGet("order-item-type")]
        public async Task<IActionResult> OrderItemType(string internalOrgId, CatalogueItemType? orderType = null)
        {
            var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);

            var model = new OrderItemTypeModel(organisation.Name)
            {
                BackLink = Url.Action(
                    nameof(DashboardController.Organisation),
                    typeof(DashboardController).ControllerName(),
                    new { internalOrgId }),
                SelectedOrderItemType = orderType,
            };

            return View(model);
        }

        [HttpPost("order-item-type")]
        public IActionResult OrderItemType(string internalOrgId, OrderItemTypeModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction(
                nameof(SelectOrganisation),
                new { internalOrgId, orderType = model.SelectedOrderItemType!.Value });
        }

        [HttpGet("proxy-select")]
        public async Task<IActionResult> SelectOrganisation(string internalOrgId, OrderTriageValue? option = null, CatalogueItemType? orderType = null)
        {
            if (!User.GetSecondaryOrganisationInternalIdentifiers().Any())
                return RedirectToAction(nameof(Index), new { internalOrgId, option, orderType });

            var internalOrgIds = new List<string>(User.GetSecondaryOrganisationInternalIdentifiers())
            {
                User.GetPrimaryOrganisationInternalIdentifier(),
            };

            var organisations = await organisationsService.GetOrganisationsByInternalIdentifiers(internalOrgIds.ToArray());

            var model = new SelectOrganisationModel(internalOrgId, organisations)
            {
                BackLink = Url.Action(nameof(OrderItemType), new { internalOrgId, orderType }),
                Title = "Which organisation are you ordering for?",
            };

            return View(model);
        }

        [HttpPost("proxy-select")]
        public IActionResult SelectOrganisation(string internalOrgId, SelectOrganisationModel model, OrderTriageValue? option = null, CatalogueItemType? orderType = null)
        {
            if (!User.GetSecondaryOrganisationInternalIdentifiers().Any())
                return RedirectToAction(nameof(Index), new { internalOrgId, option, orderType });

            if (!ModelState.IsValid)
                return View(model);

            if (!string.Equals(internalOrgId, model.SelectedOrganisation, StringComparison.OrdinalIgnoreCase))
                option = null;

            return RedirectToAction(nameof(Index), new { internalOrgId = model.SelectedOrganisation, option, orderType });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string internalOrgId, OrderTriageValue? option = null, CatalogueItemType? orderType = null)
        {
            if (orderType == CatalogueItemType.AssociatedService)
                return RedirectToAction(nameof(OrderController.ReadyToStart), typeof(OrderController).ControllerName(), new { internalOrgId, orderType });

            var backlink = User.GetSecondaryOrganisationInternalIdentifiers().Any()
                ? Url.Action(
                    nameof(SelectOrganisation),
                    new { internalOrgId, option, orderType })
                : Url.Action(
                    nameof(OrderItemType),
                    new { internalOrgId, orderType });

            var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
            var model = new OrderTriageModel(organisation)
            {
                BackLink = backlink,
                SelectedOrderTriageValue = option,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string internalOrgId, OrderTriageModel model, CatalogueItemType? orderType = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.SelectedOrderTriageValue == OrderTriageValue.NotSure)
                return RedirectToAction(nameof(NotSure), new { internalOrgId });

            return RedirectToAction(nameof(TriageSelection), new { internalOrgId, option = model.SelectedOrderTriageValue, orderType });
        }

        [HttpGet("not-sure")]
        public async Task<IActionResult> NotSure(string internalOrgId)
        {
            var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
            var model = new GenericOrderTriageModel(organisation)
            {
                BackLink = Url.Action(nameof(Index), new { internalOrgId, option = OrderTriageValue.NotSure }),
                InternalOrgId = internalOrgId,
                OrdersDashboardLink = Url.Action(
                    nameof(DashboardController.Organisation),
                    typeof(DashboardController).ControllerName(),
                    new { internalOrgId }),
            };

            return View(model);
        }

        [HttpGet("{option}")]
        public async Task<IActionResult> TriageSelection(string internalOrgId, OrderTriageValue? option, bool? selected = null, CatalogueItemType? orderType = null)
        {
            if (option is null)
                return RedirectToAction(nameof(Index), new { internalOrgId });

            var (title, advice, _) = GetTriageSelectionContent(option!.Value);

            var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
            var model = new TriageDueDiligenceModel(organisation)
            {
                BackLink = Url.Action(nameof(Index), new { internalOrgId, option, orderType }),
                Advice = advice,
                Title = title,
                Selected = selected,
                Option = option,
            };

            return View(model);
        }

        [HttpPost("{option}")]
        public IActionResult TriageSelection(string internalOrgId, TriageDueDiligenceModel model, OrderTriageValue? option, CatalogueItemType? orderType = null)
        {
            if (!model.Selected.HasValue)
            {
                var (_, _, validationError) = GetTriageSelectionContent(option!.Value);
                ModelState.AddModelError(nameof(model.Selected), validationError);
            }

            if (!ModelState.IsValid)
                return View(model);

            if (!model.Selected.GetValueOrDefault())
                return RedirectToAction(nameof(StepsNotCompleted), new { internalOrgId, option });

            return RedirectToAction(
                   nameof(OrderController.ReadyToStart),
                   typeof(OrderController).ControllerName(),
                   new { internalOrgId, option, orderType });
        }

        [HttpGet("{option}/steps-incomplete")]
        public async Task<IActionResult> StepsNotCompleted(string internalOrgId, OrderTriageValue option)
        {
            var viewName = option switch
            {
                OrderTriageValue.Under40K => "Incomplete40k",
                OrderTriageValue.Between40KTo250K => "Incomplete40kTo250k",
                OrderTriageValue.Over250K => "IncompleteOver250k",
                _ => throw new KeyNotFoundException(),
            };

            var organisation = await organisationsService.GetOrganisationByInternalIdentifier(internalOrgId);
            var model = new GenericOrderTriageModel(organisation)
            {
                BackLink = Url.Action(nameof(TriageSelection), new { internalOrgId, option, selected = false }),
                InternalOrgId = internalOrgId,
                OrdersDashboardLink = Url.Action(
                    nameof(DashboardController.Organisation),
                    typeof(DashboardController).ControllerName(),
                    new { internalOrgId }),
            };

            return View(viewName, model);
        }

        private static (string Title, string Advice, string ValidationError) GetTriageSelectionContent(OrderTriageValue option)
        {
            if (!TriageSelectionContent.TryGetValue(option, out var content))
                throw new KeyNotFoundException($"Key '{option}' is not a valid triage selection");

            return content;
        }
    }
}
