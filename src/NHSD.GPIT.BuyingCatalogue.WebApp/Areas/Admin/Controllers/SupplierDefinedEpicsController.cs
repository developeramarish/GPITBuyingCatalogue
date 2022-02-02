﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Capabilities;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Models.SupplierDefinedEpics;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.SupplierDefinedEpics;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Area("Admin")]
    [Route("admin/supplier-defined-epics")]
    public class SupplierDefinedEpicsController : Controller
    {
        private readonly ISupplierDefinedEpicsService supplierDefinedEpicsService;
        private readonly ICapabilitiesService capabilitiesService;

        public SupplierDefinedEpicsController(
            ISupplierDefinedEpicsService supplierDefinedEpicsService,
            ICapabilitiesService capabilitiesService)
        {
            this.supplierDefinedEpicsService = supplierDefinedEpicsService ?? throw new ArgumentNullException(nameof(supplierDefinedEpicsService));
            this.capabilitiesService = capabilitiesService ?? throw new ArgumentNullException(nameof(capabilitiesService));
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var supplierDefinedEpics = await supplierDefinedEpicsService.GetSupplierDefinedEpics();

            var model = new SupplierDefinedEpicsDashboardModel(supplierDefinedEpics);

            return View(model);
        }

        [HttpGet("add-epic")]
        public async Task<IActionResult> AddEpic()
        {
            var capabilities = await capabilitiesService.GetCapabilities();
            var model = new SupplierDefinedEpicBaseModel()
            {
                BackLink = Url.Action(nameof(Dashboard)),
            };

            return View(model.WithSelectListCapabilities(capabilities));
        }

        [HttpPost("add-epic")]
        public async Task<IActionResult> AddEpic(SupplierDefinedEpicBaseModel model)
        {
            if (!ModelState.IsValid)
            {
                var capabilities = await capabilitiesService.GetCapabilities();
                return View(model.WithSelectListCapabilities(capabilities));
            }

            var createEpicModel = new AddEditSupplierDefinedEpic(
                model.SelectedCapabilityId!.Value,
                model.Name,
                model.Description,
                model.IsActive!.Value);

            await supplierDefinedEpicsService.AddSupplierDefinedEpic(createEpicModel);

            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("edit/{epicId}")]
        public async Task<IActionResult> EditEpic(string epicId)
        {
            var epic = await supplierDefinedEpicsService.GetEpic(epicId);
            if (epic is null)
                return BadRequest($"No Supplier defined Epic found for Id: {epicId}");

            var capabilities = await capabilitiesService.GetCapabilities();
            var relatedItems = await supplierDefinedEpicsService.GetItemsReferencingEpic(epicId);

            var model = new EditSupplierDefinedEpicModel(epic, relatedItems)
            {
                BackLink = Url.Action(nameof(Dashboard)),
            };

            return View(model.WithSelectListCapabilities(capabilities));
        }

        [HttpPost("edit/{epicId}")]
        public async Task<IActionResult> EditEpic(string epicId, EditSupplierDefinedEpicModel model)
        {
            if (!ModelState.IsValid)
            {
                var capabilities = await capabilitiesService.GetCapabilities();
                var relatedSolutions = await supplierDefinedEpicsService.GetItemsReferencingEpic(epicId);
                model.RelatedItems = relatedSolutions;

                return View(model.WithSelectListCapabilities(capabilities));
            }

            var editEpicModel = new AddEditSupplierDefinedEpic(
                epicId,
                model.SelectedCapabilityId!.Value,
                model.Name,
                model.Description,
                model.IsActive!.Value);

            await supplierDefinedEpicsService.EditSupplierDefinedEpic(editEpicModel);

            return RedirectToAction(nameof(Dashboard));
        }
    }
}