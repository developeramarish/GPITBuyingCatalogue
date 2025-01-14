﻿using System;
using System.Collections.Generic;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Ordering.SolutionSelection.TaskList.Base;
using NHSD.GPIT.BuyingCatalogue.E2ETests.Utils;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Controllers.SolutionSelection;

namespace NHSD.GPIT.BuyingCatalogue.E2ETests.Areas.Ordering.SolutionSelection.TaskList
{
    public class TaskListAssociatedServicesOnlyOrder : TaskListBase
    {
        private const string InternalOrgId = "CG-03F";
        private const int OrderId = 90014;
        private static readonly CallOffId CallOffId = new(OrderId, 1);

        private static readonly Dictionary<string, string> Parameters = new()
        {
            { nameof(InternalOrgId), InternalOrgId },
            { nameof(CallOffId), $"{CallOffId}" },
        };

        public TaskListAssociatedServicesOnlyOrder(LocalWebApplicationFactory factory)
            : base(factory, Parameters)
        {
        }

        protected override string PageTitle => "Edit solutions and services - Order C090014-01";

        protected override bool AdditionalServicesDisplayed => false;

        protected override bool ChangeAdditionalServicesLinkVisible => false;

        protected override List<TaskListOrderItem> OrderItems => new()
        {
            new TaskListOrderItem
            {
                Name = "E2E Multiple Prices Associated Service",
                CatalogueItemId = new CatalogueItemId(99998, "S-997"),
                ServiceRecipientsAction = nameof(ServiceRecipientsController.EditServiceRecipients),
                PriceLinkActive = true,
                PriceAction = nameof(PricesController.SelectPrice),
                QuantityLinkActive = true,
                QuantityAction = nameof(QuantityController.SelectServiceRecipientQuantity),
            },
        };

        protected override Type ContinueController => typeof(ReviewSolutionsController);

        protected override string ContinueAction => nameof(ReviewSolutionsController.ReviewSolutions);

        protected override string ChangeSolutionAction => nameof(CatalogueSolutionsController.EditSolutionAssociatedServicesOnly);
    }
}
