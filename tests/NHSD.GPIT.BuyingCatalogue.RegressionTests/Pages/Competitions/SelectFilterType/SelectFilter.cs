﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Competitions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Ordering;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.RegressionTests.Utils;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Competitions.Controllers;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.RegressionTests.Pages.Competitions.SelectFilterType
{
    public class SelectFilter : PageBase
    {
        public SelectFilter(IWebDriver driver, CommonActions commonActions)
            : base(driver, commonActions)
        {
        }

        public void SelectFilterForNewCompetition(string value)
        {
            CommonActions.LedeText().Should().Be("The results produced by your selected filter can be taken into your competition.".FormatForComparison());
            CommonActions.PageLoadedCorrectGetIndex(
               typeof(CompetitionsDashboardController),
               nameof(CompetitionsDashboardController.SelectFilter))
           .Should()
           .BeTrue();

            CommonActions.ClickDropDownListWIthValue(value);
            CommonActions.ClickSave();
        }
    }
}
