﻿using System;
using System.Collections.Generic;

#nullable disable

namespace NHSD.GPIT.BuyingCatalogue.EntityFramework.Models.Ordering
{
    public partial class OrderProgress
    {
        public int OrderId { get; set; }
        public bool ServiceRecipientsViewed { get; set; }
        public bool CatalogueSolutionsViewed { get; set; }
        public bool AdditionalServicesViewed { get; set; }
        public bool AssociatedServicesViewed { get; set; }

        public virtual Order Order { get; set; }
    }
}