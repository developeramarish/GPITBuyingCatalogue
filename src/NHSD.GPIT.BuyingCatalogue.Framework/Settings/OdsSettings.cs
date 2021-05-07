﻿using System.Diagnostics.CodeAnalysis;

namespace NHSD.GPIT.BuyingCatalogue.Framework.Settings
{
    [ExcludeFromCodeCoverage]
    public sealed class OdsSettings
    {
        public string ApiBaseUrl { get; set; }

        public string[] BuyerOrganisationRoleIds { get; set; }

        public string GpPracticeRoleId { get; set; }

        public int GetChildOrganisationSearchLimit { get; set; }
    }
}
