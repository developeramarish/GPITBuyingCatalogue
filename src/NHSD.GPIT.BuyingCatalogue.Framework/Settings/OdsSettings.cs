﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Organisations.Models;

namespace NHSD.GPIT.BuyingCatalogue.Framework.Settings
{
    [ExcludeFromCodeCoverage(Justification = "Class currently only contains automatic properties")]
    public sealed class OdsSettings
    {
        public Uri ApiBaseUrl { get; init; }

        public IReadOnlyList<OrganisationRoleSettings> BuyerOrganisationRoles { get; init; }


        public string SubLocationRoleId { get; set; }

        public string IsCommissionedByRelType { get; set; }

        public string InGeographyOfRelType { get; set; }


        public OrganisationType GetOrganisationType(string primaryRoleId)
        {
            if (string.IsNullOrEmpty(primaryRoleId))
                throw new ArgumentNullException(nameof(primaryRoleId));

            var role = BuyerOrganisationRoles.FirstOrDefault(x => x.PrimaryRoleId == primaryRoleId);

            if (role is null)
                throw new ArgumentException("Not a buyer role", nameof(primaryRoleId));

            return role.OrganisationType;
        }

        public string GetPrimaryRoleId(OrganisationType orgType)
        {
            var role = BuyerOrganisationRoles.FirstOrDefault(x => x.OrganisationType == orgType);

            if (role is null)
                throw new ArgumentException("Not a buyer role", nameof(orgType));

            return role.PrimaryRoleId;
        }
    }
}
