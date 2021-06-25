﻿using NHSD.GPIT.BuyingCatalogue.EntityFramework.Ordering.Models;

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.GPITBuyingCatalogue
{
    public sealed class SupplierServiceAssociation
    {
        public CatalogueItemId AssociatedServiceId { get; set; }

        public CatalogueItemId CatalogueItemId { get; set; }

        public AssociatedService AssociatedService { get; set; }

        public CatalogueItem CatalogueItem { get; set; }
    }
}
