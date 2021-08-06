﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Solutions;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Area("Admin")]
    [Route("admin")]
    public sealed class HomeController : Controller
    {
        private readonly IOrganisationsService organisationsService;
        private readonly IMapper mapper;
        private readonly ISolutionsService solutionsService;

        public HomeController(
            IOrganisationsService organisationsService,
            IMapper mapper,
            ISolutionsService solutionsService)
        {
            this.organisationsService = organisationsService ?? throw new ArgumentNullException(nameof(organisationsService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.solutionsService = solutionsService ?? throw new ArgumentNullException(nameof(solutionsService));
        }

        [HttpGet("buyer-organisations")]
        public async Task<IActionResult> BuyerOrganisations()
        {
            var organisations = await organisationsService.GetAllOrganisations();
            var organisationModel = mapper.Map<IList<Organisation>, IList<OrganisationModel>>(organisations);

            return View(new ListOrganisationsModel(organisationModel));
        }

        [HttpGet("manage-suppliers")]
        public async Task<IActionResult> ManageSuppliers()
        {
            var suppliers = await solutionsService.GetAllSuppliers();
            return View(suppliers);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
