﻿using NHSD.GPIT.BuyingCatalogue.WebApp.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models
{
    public sealed class ConfirmationModel : NavBaseModel
    {
        public ConfirmationModel(string organisationName)
        {
            Name = organisationName;
            BackLink = "/admin/buyer-organisations";
            BackLinkText = "Back to dashboard";
        }

        public string Name { get; set; }
    }
}
