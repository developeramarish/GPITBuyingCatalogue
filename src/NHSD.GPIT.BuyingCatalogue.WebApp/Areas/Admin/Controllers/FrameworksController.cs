using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Frameworks;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models.FrameworkModels;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Controllers;

[Authorize(Policy = "AdminOnly")]
[Area("Admin")]
[Route("admin/frameworks")]
public class FrameworksController : Controller
{
    private readonly IFrameworkService frameworkService;

    public FrameworksController(
        IFrameworkService frameworkService)
    {
        this.frameworkService = frameworkService ?? throw new ArgumentNullException(nameof(frameworkService));
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var frameworks = await frameworkService.GetFrameworks();

        var model = new FrameworksDashboardModel(frameworks);

        return View(model);
    }

    [HttpGet("add")]
    public IActionResult Add() => View(new AddFrameworkModel { BackLink = Url.Action(nameof(Dashboard)) });

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddFrameworkModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await frameworkService.AddFramework(model.Name, model.IsLocalFundingOnly.GetValueOrDefault());

        return RedirectToAction(nameof(Dashboard));
    }

    [HttpGet("expire/{frameworkId}")]
    public async Task<IActionResult> Expire(string frameworkId)
    {
        var framework = await frameworkService.GetFramework(frameworkId);
        if (framework is null)
            return RedirectToAction(nameof(Dashboard));

        var model = new ExpireFrameworkModel
        {
            Name = framework.ShortName, BackLink = Url.Action(nameof(Dashboard), new { frameworkId }),
        };

        return View(model);
    }

    [HttpPost("expire/{frameworkId}")]
    public async Task<IActionResult> Expire(string frameworkId, ExpireFrameworkModel model)
    {
        _ = model;

        await frameworkService.MarkAsExpired(frameworkId);

        return RedirectToAction(nameof(Dashboard));
    }
}
