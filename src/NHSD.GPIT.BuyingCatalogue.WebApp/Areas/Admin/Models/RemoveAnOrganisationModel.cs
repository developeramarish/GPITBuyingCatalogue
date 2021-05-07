﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Identity;
using NHSD.GPIT.BuyingCatalogue.Framework.Extensions;
using NHSD.GPIT.BuyingCatalogue.ServiceContracts.Organisations;
using NHSD.GPIT.BuyingCatalogue.WebApp.Models;

namespace NHSD.GPIT.BuyingCatalogue.WebApp.Areas.Admin.Models
{
    public class RemoveAnOrganisationModel : NavBaseModel
    {
        public RemoveAnOrganisationModel()
        {
        }

        public RemoveAnOrganisationModel(Guid organisationId, Organisation relatedOrganisation)
        {
            OrganisationId = organisationId;
            RelatedOrganisation = relatedOrganisation;            
            BackLink = $"/admin/organisations/{organisationId}";
        }

        public Guid OrganisationId { get; set; }

        public Organisation RelatedOrganisation { get; set; }

        public List<Organisation> AvailableOrganisations { get; set; }    
        
        public Guid SelectedOrganisation { get; set; }
    }
}
