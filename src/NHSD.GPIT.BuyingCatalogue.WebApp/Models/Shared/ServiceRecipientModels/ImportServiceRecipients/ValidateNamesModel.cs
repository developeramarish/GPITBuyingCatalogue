﻿using System.Collections.Generic;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Orders.Models.SolutionSelection.ServiceRecipients;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Models.Shared.ServiceRecipientModels.ImportServiceRecipients;

public class ValidateNamesModel : NavBaseModel
{
    public ValidateNamesModel()
    {
    }

    public ValidateNamesModel(
        IEnumerable<(string Expected, string Actual, string OdsCode)> mismatchedNames,
        ServiceRecipientImportMode? importMode = null)
    {
        ImportMode = importMode;

        NameDiscrepancies = mismatchedNames.Select(p => new ServiceRecipientNameDiscrepancy(p.Expected, p.Actual, p.OdsCode)).ToList();
    }

    public string CancelLink { get; set; }

    public ServiceRecipientImportMode? ImportMode { get; set; }

    public IList<ServiceRecipientNameDiscrepancy> NameDiscrepancies { get; set; }

    public class ServiceRecipientNameDiscrepancy
    {
        public ServiceRecipientNameDiscrepancy()
        {
        }

        public ServiceRecipientNameDiscrepancy(
            string expectedName,
            string actualName,
            string odsCode)
        {
            ExpectedName = expectedName;
            ActualName = actualName;
            OdsCode = odsCode;
        }

        public string ActualName { get; set; }

        public string ExpectedName { get; set; }

        public string OdsCode { get; set; }
    }
}
