﻿using System;
using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Catalogue.Models
{
    public class CatalogueItemCapability
    {
        public CatalogueItemId CatalogueItemId { get; set; }

        public Guid CapabilityId { get; set; }

        public int StatusId { get; set; }

        public DateTime LastUpdated { get; set; }

        public Guid LastUpdatedBy { get; set; }

        public Capability Capability { get; set; }

        public CatalogueItemCapabilityStatus Status { get; set; }
    }
}