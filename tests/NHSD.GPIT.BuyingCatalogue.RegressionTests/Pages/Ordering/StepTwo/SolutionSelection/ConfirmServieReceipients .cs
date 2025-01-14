﻿using Azure.Storage.Blobs.Models;
using FluentAssertions;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Actions.Common;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Framework.Objects.Ordering;
using NHSD.GPIT.BuyingCatalogue.RegressionTests.Utils;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers.SolutionSelection;
using OpenQA.Selenium;

namespace NHSD.GPIT.BuyingCatalogue.RegressionTests.Pages.Ordering.StepTwo.SolutionSelection
{
    internal class ConfirmServieReceipients : PageBase
    {
        public ConfirmServieReceipients(IWebDriver driver, CommonActions commonActions)
            : base(driver, commonActions)
        {
        }

        public void ConfirmServiceReceipientsChanges()
        {
            CommonActions.PageLoadedCorrectGetIndex(
                typeof(ServiceRecipientsController),
                nameof(ServiceRecipientsController.ConfirmChanges)).Should().BeTrue();

            CommonActions.ClickSave();
        }
    }
}
