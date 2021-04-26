﻿using System;
using System.Collections.Generic;

#nullable disable

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Ordering
{
    public partial class CatalogueItem
    {
        public CatalogueItem()
        {
            InverseParentCatalogueItem = new HashSet<CatalogueItem>();
            OrderItems = new HashSet<OrderItem>();
        }

        public string Id { get; set; }
        public int CatalogueItemTypeId { get; set; }
        public string Name { get; set; }
        public string ParentCatalogueItemId { get; set; }

        public virtual CatalogueItemType CatalogueItemType { get; set; }
        public virtual CatalogueItem ParentCatalogueItem { get; set; }
        public virtual ICollection<CatalogueItem> InverseParentCatalogueItem { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}