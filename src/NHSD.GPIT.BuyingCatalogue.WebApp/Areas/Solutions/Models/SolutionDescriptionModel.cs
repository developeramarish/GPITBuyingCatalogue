﻿namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Solutions.Models
{
    public class SolutionDescriptionModel : SolutionDisplayBaseModel
    {
        public string Description { get; set; }

        public string Framework { get; set; }

        public string IsFoundation { get; set; }

        public override string Section { get; set; }

        public string Summary { get; set; }

        public string SupplierName { get; set; }
    }
}